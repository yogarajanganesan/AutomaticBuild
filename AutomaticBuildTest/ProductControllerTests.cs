using Xunit;
using AutomaticBuild.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AutomaticBuild;

public class ProductControllerTests
{
    [Fact]
    public void Get_ReturnsListOfProducts()
    {
        var controller = new ProductController();

        var result = controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        var productList = products.ToList();

        Assert.Equal(3, productList.Count);
        Assert.Contains(productList, p => p.Name == "Laptop");
        Assert.Contains(productList, p => p.Name == "Mouse");
        Assert.Contains(productList, p => p.Name == "Keyboard");
    }

    [Fact]
    public void Get_ProductsHaveUniqueIds()
    {
        var controller = new ProductController();

        var result = controller.Get();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        var productList = products.ToList();

        var uniqueIds = productList.Select(p => p.Id).Distinct().Count();
        Assert.Equal(productList.Count, uniqueIds);
    }

    [Fact]
    public void Get_ProductsHavePositivePrices()
    {
        var controller = new ProductController();

        var result = controller.Get();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

        Assert.All(products, p => Assert.True(p.Price > 0));
    }

    [Fact]
    public void Get_ProductListIsNotEmpty()
    {
        var controller = new ProductController();

        var result = controller.Get();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

        Assert.NotEmpty(products);
    }

    [Fact]
    public void Get_ProductNameIsNotNullOrEmpty()
    {
        var controller = new ProductController();

        var result = controller.Get();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

        Assert.All(products, p => Assert.False(string.IsNullOrEmpty(p.Name)));
    }

    // Negative scenario: Simulate a controller with no products (for demonstration)
    [Fact]
    public void Get_WhenNoProducts_ReturnsEmptyList()
    {
        var controller = new EmptyProductController();

        var result = controller.Get();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

        Assert.Empty(products);
    }
}

// Helper controller for negative scenario
public class EmptyProductController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        return Ok(new List<Product>());
    }
}
