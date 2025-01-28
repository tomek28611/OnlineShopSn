using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineShop.Areas.Admin.Controllers;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Models.Db;

namespace OnlineShop.Tests.Areas.Admin.Controllers
{
    public class MenusControllerTest
    {
        private readonly Mock<IMenusService> _menusServiceMock;
        private readonly MenusController _controller;

        public MenusControllerTest()
        {
            _menusServiceMock = new Mock<IMenusService>();
            _controller = new MenusController(_menusServiceMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfMenus()
        {
            // Arrange
            var menus = new List<Menus>
            {
                new Menus { Id = 1, MenuTitle = "Menu1", Link = "Link1", Type = "Type1" },
                new Menus { Id = 2, MenuTitle = "Menu2", Link = "Link2", Type = "Type2" }
            };
            _menusServiceMock.Setup(service => service.GetAllMenusAsync())
                .ReturnsAsync(menus);
            // Act
            var result = await _controller.Index();
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(menus, viewResult.Model);
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
            _menusServiceMock.Setup(service => service.GetMenuDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync((Menus)null);
            // Act
            var result = await _controller.Details(1);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidModel_ReturnsRedirectToAction()
        {
            // Arrange
            var menu = new Menus { Id = 1, MenuTitle = "Menu1", Link = "Link1", Type = "Type1" };
            // Act
            var result = await _controller.Create(menu);
            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Edit_ValidModel_ReturnsRedirectToAction()
        {
            // Arrange
            var menu = new Menus { Id = 1, MenuTitle = "Menu1", Link = "Link1", Type = "Type1" };
            // Act
            var result = await _controller.Edit(1, menu);
            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Delete_ValidId_ReturnsViewResult_WithMenu()
        {
            // Arrange
            var menu = new Menus { Id = 1, MenuTitle = "Menu1", Link = "Link1", Type = "Type1" };
            _menusServiceMock.Setup(service => service.GetMenuForDeleteAsync(1))
                .ReturnsAsync(menu);
            // Act
            var result = await _controller.Delete(1);
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(menu, viewResult.Model);
        }
        [Fact]
        public async Task DeleteConfirmed_ValidId_ReturnsRedirectToAction()
        {
            // Act
            var result = await _controller.DeleteConfirmed(1);
            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

    }
}
