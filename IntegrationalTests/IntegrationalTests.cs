using Xunit;

namespace IntegrationalTests;

public class IntegrationalTests : IClassFixture<ServiceWebApplicationFactory<Program>>
{
    private readonly ServiceWebApplicationFactory<Program> _factory;

    public IntegrationalTests(ServiceWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetOrder_ById_NotFound_WhenOrderDoesNotExist()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"api/orders/1");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}