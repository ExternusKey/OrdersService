using Common.Models;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProcessingService.Kafka.Interfaces;
using ProcessingService.Models.Responses;
using ProcessingService.Services;
using ProcessingService.Services.Interfaces;
using Xunit;

namespace UnitTests.OrderDaemonTests;

public class ProductsServiceTests
{
    private readonly Mock<IProcessingServiceProducer> _mockProducer;
    private readonly OrderDbContext _dbContext;
    private readonly IProductsService _productsService;

    public ProductsServiceTests()
    {
        _mockProducer = new Mock<IProcessingServiceProducer>();
        Mock<ILogger<ProductsService>> mockLogger = new Mock<ILogger<ProductsService>>();

        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new OrderDbContext(options);

        _productsService = new ProductsService(_dbContext, _mockProducer.Object, mockLogger.Object);
    }

    [Fact]
    public async Task CheckProductAvailabilityAsync_ShouldReturnProductNotFound_WhenProductDoesNotExist()
    {
        var result = await _productsService.CheckProductAvailabilityAsync(2);

        Assert.Equal(ProductResponseStatus.ProductNotFound, result.Status);
        Assert.Equal("Product not found", result.Message);
    }

    [Fact]
    public async Task CheckProductAvailabilityAsync_ShouldReturnSuccess_WhenProductExists()
    {
        var product = new Product { Id = 2, Name = "RTX", Amount = 100 };
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var result = await _productsService.CheckProductAvailabilityAsync(1);

        Assert.Equal(ProductResponseStatus.Success, result.Status);
    }

    [Fact]
    public async Task UpdateProductAmountAsync_ShouldReturnNotEnoughAmount_WhenAmountIsGreaterThanAvailable()
    {
        _dbContext.Products.RemoveRange(_dbContext.Products);
        await _dbContext.SaveChangesAsync();

        var product = new Product { Id = 1, Name = "RTX", Amount = 10 };
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var result = await _productsService.UpdateProductAmountAsync(1, 20);

        Assert.Equal(ProductResponseStatus.NotEnoughAmount, result.Status);
        Assert.Equal("Not enough product amount", result.Message);
    }

    [Fact]
    public async Task UpdateProductAmountAsync_ShouldReturnSuccess_WhenAmountIsValid()
    {
        _dbContext.Products.RemoveRange(_dbContext.Products);
        await _dbContext.SaveChangesAsync();

        var product = new Product { Id = 1, Name = "RTX", Amount = 10 };
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var result = await _productsService.UpdateProductAmountAsync(1, 5);

        Assert.Equal(ProductResponseStatus.Success, result.Status);
        var updatedProduct = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == 1);
        if (updatedProduct != null) Assert.Equal(5, updatedProduct.Amount);
    }

    [Fact]
    public async Task AddOrderAsync_ShouldAddConfirmedOrder_WhenStatusIsTrue()
    {
        var order = new Order { Id = 1 };
        var confirmedOrderAdded = new ConfirmedOrder { Id = 3, OrderStatus = "Confirmed" };
        _dbContext.ConfirmedOrders.Add(confirmedOrderAdded);
        await _dbContext.SaveChangesAsync();

        await _productsService.AddOrderAsync(order, true);

        var confirmedOrder = await _dbContext.ConfirmedOrders.SingleOrDefaultAsync(co => co.OrderId == order.Id);
        Assert.NotNull(confirmedOrder);
        Assert.Equal("Confirmed", confirmedOrder.OrderStatus);
        _mockProducer.Verify(p => p.SendStatusOrderAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task AddOrderAsync_ShouldAddRejectedOrder_WhenStatusIsFalse()
    {
        var order = new Order { Id = 2 };
        var rejectedOrderAdded = new RejectedOrder { Id = 3 };
        _dbContext.RejectedOrders.Add(rejectedOrderAdded);
        await _dbContext.SaveChangesAsync();

        await _productsService.AddOrderAsync(order, false, "Out of stock");

        var rejectedOrder = await _dbContext.RejectedOrders.SingleOrDefaultAsync(ro => ro.OrderId == order.Id);
        Assert.NotNull(rejectedOrder);
        Assert.Equal("Out of stock", rejectedOrder.RejectionReason);
        _mockProducer.Verify(p => p.SendStatusOrderAsync(It.IsAny<string>()), Times.Once);
    }
}