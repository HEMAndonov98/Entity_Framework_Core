using System.Collections;
using AspNetCoreTemplate.Services;
using Blog.Data;
using Blog.Data.Common.Repositories;
using Blog.Data.Models;
using Blog.Web.ViewModels.Article;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework.Internal;

namespace ArticleServiceTests;

public class ArticleServiceTests
{

    private Mock<IRepository<Article>> articleRepoMock;
    private readonly DbContextOptions<ApplicationDbContext> contextOptions =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "BlogTests")
            .Options;
    
    private  ArticleService sut;
    private ApplicationDbContext testContext;
    
    
    [OneTimeSetUp]
    public void SetUp()
    {
        this.testContext = new ApplicationDbContext(this.contextOptions);
        this.articleRepoMock = new Mock<IRepository<Article>>();
        this.sut = new ArticleService(this.articleRepoMock.Object, this.testContext);
    }

    [Test]
    public async Task Add_ShouldAddArticleToDatabase()
    {
        //Arrange
        ArticleAddViewModel addViewModel = new ArticleAddViewModel()
        {
            Title = "TestTitle",
            Content = "TestContent",
            CategoryId = 1,
            Author = "TestAuthor",
        };
        
        
        this.articleRepoMock.Setup(x => x.AddAsync(It.IsAny<Article>())).Verifiable();
        //Act

        await this.sut.AddArticle(addViewModel);
        //Assert

        this.articleRepoMock.Verify(x => x.AddAsync(It.Is<Article>(a => a.Title == "TestTitle")), Times.Once);
        this.articleRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task GetAll_ShouldReturnIEnumerableOfArticleViewModel()
    {
        //Arrange
        var articles = new Article[3]
        {
            new Article() {Id = 0, Title = "TestArticle1" },
            new Article() {Id = 1, Title = "TestArticle2" },
            new Article() {Id = 3, Title = "TestArticle3" },
        };

        this.articleRepoMock.Setup(x => x.AllIncludingAsNoTracking(x => x.Category))
            .Returns(articles.AsQueryable());
        //Act
        var real = await this.sut.GetAllAsync();
        var actual = real.ToList();
        //Assert

        Assert.NotNull(actual);
        Assert.That(actual.Count(), Is.EqualTo(articles.Length));

        var articleIndex = 0;
        
        foreach (var articleViewModel in actual)
        {
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<ArticleViewModel>(articleViewModel);
                Assert.That(articleViewModel.Id, Is.EqualTo(articles[articleIndex].Id));
                Assert.That(articleViewModel.Title, Is.EqualTo(articles[articleIndex].Title));
            });
            
            articleIndex++;
        }
    }
}