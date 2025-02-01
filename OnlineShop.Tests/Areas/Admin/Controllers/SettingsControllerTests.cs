
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineShop.Areas.Admin.Controllers;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Data;

public class SettingsControllerTests
{
    private readonly Mock<ISettingsService> _mockSettingsService;
    private readonly SettingsController _settingsController;

    public SettingsControllerTests()
    {
        _mockSettingsService = new Mock<ISettingsService>();
        _settingsController = new SettingsController(_mockSettingsService.Object);
    }

    [Fact]
    public async Task Edit_Get_ReturnsViewWithSetting_WhenSettingExists()
    {
        // Arrange
        var setting = new Setting { Id = 1, Title = "Test Title" };
        _mockSettingsService.Setup(s => s.GetSettingAsync()).ReturnsAsync(setting);

        // Act
        var result = await _settingsController.Edit();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Setting>(viewResult.Model);
        Assert.Equal(setting.Id, model.Id);
        Assert.Equal(setting.Title, model.Title);
    }

    [Fact]
    public async Task Edit_Get_ReturnsNotFound_WhenSettingIsNull()
    {
        // Arrange
        _mockSettingsService.Setup(s => s.GetSettingAsync()).ReturnsAsync((Setting)null);

        // Act
        var result = await _settingsController.Edit();

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Post_ReturnsView_WhenModelStateIsInvalid()
    {
        // Arrange
        var setting = new Setting { Id = 1, Title = "Test Title" };
        _settingsController.ModelState.AddModelError("Error", "Invalid model");

        // Act
        var result = await _settingsController.Edit(setting.Id, setting, null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Setting>(viewResult.Model);
        Assert.Equal(setting.Id, model.Id);
    }

    [Fact]
    public async Task Edit_Post_ReturnsNotFound_WhenIdMismatch()
    {
        // Arrange
        var setting = new Setting { Id = 1, Title = "Test Title" };

        // Act
        var result = await _settingsController.Edit(2, setting, null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }


}

