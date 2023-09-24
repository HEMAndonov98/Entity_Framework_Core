using System.Linq.Expressions;
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

    [TearDown]
    public void TearDown()
    {
        this.mockDbContext.Invocations.Clear();
        this.articleRepoMock.Invocations.Clear();
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

    [Test]
    public async Task GetAllAsync_ShouldReturnCorrectlyMappedArticleViewModelCollection()
    {
        //Arrange
        var sampleArticles = new List<Article>
            {
                new Article { Title = "Title1", Content = "Content1", Author = "Author1" , Category = new Category {Name = "Category1"}},
                new Article { Title = "Title2", Content = "Content2", Author = "Author2", Category = new Category {Name = "Category2" }},
                new Article { Title = "Title3", Content = "Content3", Author = "Author3", Category = new Category {Name = "Category3" }},
            };
        
        var expectedResult = sampleArticles
            .Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                CreatedOn = a.CreatedOn,
                Author = a.Author,
                Category = a.Category.Name,
            })
            .ToArray();

        this.articleRepoMock.Setup(x => x.AllIncludingAsNoTracking(a => a.Category))
            .ReturnsAsync(sampleArticles);
        //Act
        var result = await this.sut.GetAllAsync();
        var actual = result.ToArray();
        //Assert
        Assert.That(actual.Count, Is.EqualTo(expectedResult.Count()));

        for (int i = 0; i < expectedResult.Length; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(expectedResult[i].Title, Is.EqualTo(actual[i].Title));
                Assert.That(expectedResult[i].Content, Is.EqualTo(actual[i].Content));
                Assert.That(expectedResult[i].Author, Is.EqualTo(actual[i].Author));
                Assert.That(expectedResult[i].Category, Is.EqualTo(actual[i].Category));
            });
        }
    }

    [Test]
    public async Task EditArticleAsync_ShouldCorrectlyModifyArticle()
    {
        //Arrange
        var editModel = new ArticleEditViewModel
        {
            Title = "Title1",
            Content = "Content1",
            Author = "Author1",
        };


        this.articleRepoMock.Setup(x => x.Update(It.IsAny<Article>()));
        this.articleRepoMock.Setup(x => x.SaveChangesAsync())
            .Verifiable();
        //Act

        await this.sut.EditArticleAsync(editModel);
        //Assert
        
        this.articleRepoMock.Verify(x => x.Update(It.IsAny<Article>()), Times.Once);
        this.articleRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task DeleteArticleAsync_ShouldDoNothingIfArticleDoesNotExist()
    {
        //Arrange
        Article? nullArticle = null;
        
        this.articleRepoMock.Setup(x => x.FindAsyncIncluding(a => a.Category, a => a.Id == -1))!
            .ReturnsAsync(nullArticle);
        
        //Act
        await this.sut.DeleteArticleAsync(-1);
        //Assert
        
        this.articleRepoMock.Verify(x => x.Delete(It.IsAny<Article>()), Times.Never);
        this.articleRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Test]
    public async Task DeleteArticleAsync_ShouldDeleteFoundArticle()
    {
        //Arrange
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
        //Act

        await this.sut.DeleteArticleAsync(1);
        //Assert
        
        this.articleRepoMock.Verify(x => x.Delete(It.IsAny<Article>()), Times.Once);
        this.articleRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}