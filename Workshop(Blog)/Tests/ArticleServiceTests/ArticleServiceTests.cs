using AspNetCoreTemplate.Services;
using Blog.Data;
using Blog.Data.Common.Repositories;
using Blog.Data.Models;
using Blog.Web.ViewModels.Article;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ArticleServiceTests;

public class ArticleServiceTests
{

    private Mock<IRepository<Article>> articleRepoMock;
    private readonly DbContextOptions<ApplicationDbContext> contextOptions =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "BlogTests")
            .Options;
    
    private  ArticleService sut;
    private Mock<ApplicationDbContext> mockDbContext;
    
    
    [OneTimeSetUp]
    public void SetUp()
    {
        this.mockDbContext = new Mock<ApplicationDbContext>(contextOptions);
        this.articleRepoMock = new Mock<IRepository<Article>>();
        this.sut = new ArticleService(this.articleRepoMock.Object, this.mockDbContext.Object);
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
    public async Task GetArticleAsync_ShouldReturnValidArticleViewModel()
    {
        var createdOn = DateTime.Now;
        
        var sampleArticle = new Article
        {
            Id = 1,
            Title = "Article 1",
            Content = "Content 1",
            Author = "Author 1",
            CreatedOn = createdOn,
            Category = new Category{Name = "Test Category"}
        };

        this.articleRepoMock.Setup(x => x.FindAsyncIncluding(a => a.Category, a => a.Id == 1))
            .ReturnsAsync(sampleArticle);
        var result = await this.sut.GetArticleAsync(1);
        
        Assert.IsNotNull(result);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(sampleArticle.Id));
            Assert.That(result.Title, Is.EqualTo(sampleArticle.Title));
            Assert.That(result.Content, Is.EqualTo(sampleArticle.Content));
            Assert.That(result.Author, Is.EqualTo(sampleArticle.Author));
            Assert.That(result.CreatedOn, Is.EqualTo(sampleArticle.CreatedOn));
            Assert.That(result.Category, Is.EqualTo(sampleArticle.Category.Name));
        });
    }
}