using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineShop.Areas.Admin.Controllers;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Data;
using Xunit;

public class UsersControllerTests
{
    [Fact]
    public async Task Index_ShouldReturnViewWithUsers()
    {
        // Arrange
        var mockService = new Mock<IUsersService>();
        mockService.Setup(service => service.GetAllUsersAsync())
            .ReturnsAsync(new List<User>
            {
                new User { Id = 1, Email = "user1@example.com" },
                new User { Id = 2, Email = "user2@example.com" }
            });

        var controller = new UsersController(mockService.Object);

        // Act
        var result = await controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsType<List<User>>(result.Model);
        Assert.Equal(2, model.Count);
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenIdIsNull()
    {
        // Arrange
        var mockService = new Mock<IUsersService>();
        var controller = new UsersController(mockService.Object);

        // Act
        var result = await controller.Details(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var mockService = new Mock<IUsersService>();
        mockService.Setup(service => service.GetUserByIdAsync(1))
            .ReturnsAsync(new User { Id = 1, Email = "user@example.com" });

        var controller = new UsersController(mockService.Object);

        // Act
        var result = await controller.Details(1) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsType<User>(result.Model);
        Assert.Equal(1, model.Id);
        Assert.Equal("user@example.com", model.Email);
    }
}
