using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineShop.Areas.Admin.Controllers;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Models.Db;
 
public class CommentsControllerTests
{
    private readonly Mock<ICommentsService> _commentsServiceMock;
    private readonly CommentsController _controller;

    public CommentsControllerTests()
    {
        _commentsServiceMock = new Mock<ICommentsService>();
        _controller = new CommentsController(_commentsServiceMock.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfComments()
    {
        // Arrange
        var comments = new List<Comment>
        {
            new Comment { Id = 1, Name = "User1", CommentText = "Comment 1" },
            new Comment { Id = 2, Name = "User2", CommentText = "Comment 2" }
        };

        _commentsServiceMock.Setup(service => service.GetAllCommentsAsync())
            .ReturnsAsync(comments);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(comments, viewResult.Model);
    }

    [Fact]
    public async Task Details_NullId_ReturnsNotFound()
    {
        // Act
        var result = await _controller.Details(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_InvalidId_ReturnsNotFound()
    {
        // Arrange
        _commentsServiceMock.Setup(service => service.GetCommentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Comment)null);

        // Act
        var result = await _controller.Details(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ValidId_ReturnsViewResult_WithComment()
    {
        // Arrange
        var comment = new Comment { Id = 1, Name = "User1", CommentText = "Comment 1" };
        _commentsServiceMock.Setup(service => service.GetCommentByIdAsync(1))
            .ReturnsAsync(comment);

        // Act
        var result = await _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(comment, viewResult.Model);
    }

    [Fact]
    public async Task Create_ValidComment_RedirectsToIndex()
    {
        // Arrange
        var comment = new Comment { Id = 1, Name = "User1", CommentText = "Comment 1" };

        // Act
        var result = await _controller.Create(comment);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _commentsServiceMock.Verify(service => service.CreateCommentAsync(comment), Times.Once);
    }

    [Fact]
    public async Task Edit_NullId_ReturnsNotFound()
    {
        // Act
        var result = await _controller.Edit(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_InvalidId_ReturnsNotFound()
    {
        // Arrange
        _commentsServiceMock.Setup(service => service.GetCommentByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Comment)null);

        // Act
        var result = await _controller.Edit(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_ValidId_ReturnsViewResult_WithComment()
    {
        // Arrange
        var comment = new Comment { Id = 1, Name = "User1", CommentText = "Comment 1" };
        _commentsServiceMock.Setup(service => service.GetCommentByIdAsync(1))
            .ReturnsAsync(comment);

        // Act
        var result = await _controller.Edit(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(comment, viewResult.Model);
    }

    [Fact]
    public async Task DeleteConfirmed_ValidId_RedirectsToIndex()
    {
        // Arrange
        _commentsServiceMock.Setup(service => service.DeleteCommentAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteConfirmed(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _commentsServiceMock.Verify(service => service.DeleteCommentAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteConfirmed_InvalidId_ReturnsNotFound()
    {
        // Arrange
        _commentsServiceMock.Setup(service => service.DeleteCommentAsync(It.IsAny<int>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteConfirmed(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
