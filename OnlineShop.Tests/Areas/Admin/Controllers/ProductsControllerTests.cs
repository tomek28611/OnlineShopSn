using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineShop.Areas.Admin.Controllers;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Models.Db;

public class ProductsControllerTests
{
    private readonly Mock<IProductsService> _productsServiceMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _productsServiceMock = new Mock<IProductsService>();
        _controller = new ProductsController(null, _productsServiceMock.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Title = "Product 1" },
            new Product { Id = 2, Title = "Product 2" }
        };

        _productsServiceMock.Setup(service => service.GetAllProductsAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(products, viewResult.Model);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WithProductDetails()
    {
        // Arrange
        var product = new Product { Id = 1, Title = "Product 1" };
        _productsServiceMock.Setup(service => service.GetProductDetailsAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(product, viewResult.Model);
    }

    [Fact]
    public async Task Create_ValidProduct_RedirectsToIndex()
    {
        // Arrange
        var product = new Product { Id = 1, Title = "Product 1" };

        // Act
        var result = await _controller.Create(product, null, null);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }
     
    [Fact]
    public async Task Edit_ValidProduct_RedirectsToIndex()
    {
        // Arrange
        var product = new Product { Id = 1, Title = "Product 1" };

        _productsServiceMock.Setup(service => service.UpdateProductAsync(product, null, null))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Edit(1, product, null, null);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task DeleteConfirmed_CallsService_DeleteProduct()
    {
        // Act
        var result = await _controller.DeleteConfirmed(1);

        // Assert
        _productsServiceMock.Verify(service => service.DeleteProductAsync(1), Times.Once);
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }
}
