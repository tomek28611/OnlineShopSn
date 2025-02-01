using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineShop.Areas.Admin.Controllers;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Data.Entities;

public class BannersControllerTests
{
    private readonly Mock<IBannerService> _bannerServiceMock;
    private readonly BannersController _controller;

    public BannersControllerTests()
    {
        _bannerServiceMock = new Mock<IBannerService>();
        _controller = new BannersController(_bannerServiceMock.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfBanners()
    {
        // Arrange
        var banners = new List<BannerEntity>
        {
            new BannerEntity { Id = 1, Title = "Banner 1" },
            new BannerEntity { Id = 2, Title = "Banner 2" }
        };

        _bannerServiceMock.Setup(service => service.GetAllBannersAsync())
            .ReturnsAsync(banners);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(banners, viewResult.Model);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WithBannerDetails()
    {
        // Arrange
        var banner = new BannerEntity { Id = 1, Title = "Banner 1" };
        _bannerServiceMock.Setup(service => service.GetBannerByIdAsync(1))
            .ReturnsAsync(banner);

        // Act
        var result = await _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(banner, viewResult.Model);
    }

    [Fact]
    public async Task Create_ValidBanner_RedirectsToIndex()
    {
        // Arrange
        var banner = new BannerEntity { Id = 1, Title = "Banner 1" };
        _bannerServiceMock.Setup(service => service.CreateBannerAsync(banner, null))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Create(banner, null);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Edit_ValidBanner_RedirectsToIndex()
    {
        // Arrange
        var banner = new BannerEntity { Id = 1, Title = "Banner 1" };
        _bannerServiceMock.Setup(service => service.UpdateBannerAsync(banner, null))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Edit(1, banner, null);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task DeleteConfirmed_CallsService_DeleteBanner()
    {
        // Arrange
        _bannerServiceMock.Setup(service => service.DeleteBannerAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteConfirmed(1);

        // Assert
        _bannerServiceMock.Verify(service => service.DeleteBannerAsync(1), Times.Once);
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }
}
