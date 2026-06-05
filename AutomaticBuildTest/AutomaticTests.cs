using Xunit;
using Microsoft.Extensions.Logging;
using AutomaticBuild.Controllers;
using System.Collections.Generic;
using System.Linq;
using AutomaticBuild;
using Microsoft.AspNetCore.Mvc;

public class AutomaticTests
{
    [Fact]
    public void Get_ReturnsFiveWeatherForecasts()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<WeatherForecastController>();
        var controller = new WeatherForecastController(logger);

        // Act
        var result = controller.Get();

        // Assert
        Assert.NotNull(result);
        var list = result.ToList();
        Assert.Equal(5, list.Count);
        Assert.All(list, item => Assert.NotNull(item.Summary));
    }

    [Fact]
    public void Get_EachForecast_HasValidTemperatureRange()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<WeatherForecastController>();
        var controller = new WeatherForecastController(logger);

        // Act
        var result = controller.Get();

        // Assert
        Assert.All(result, item =>
        {
            Assert.InRange(item.TemperatureC, -20, 54); // Random.Shared.Next is exclusive of upper bound
        });
    }

    [Fact]
    public void Get_EachForecast_HasSummaryFromList()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<WeatherForecastController>();
        var controller = new WeatherForecastController(logger);

        // Act
        var result = controller.Get();
        var summaries = new[]
        {
            "Kishan.Yogarajan", "Sujan.Yogarajan", "Lingesh.Jawahar", "Soundhar", "Bharathi", "Vishant", "Santhosh", "Yogarajan", "Yogaraja", "Yogaraj"
        };

        // Assert
        Assert.All(result, item =>
        {
            Assert.Contains(item.Summary, summaries);
        });
    }

    [Fact]
    public void Get_ReturnsDifferentResultsOnMultipleCalls()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<WeatherForecastController>();
        var controller = new WeatherForecastController(logger);

        // Act
        var result1 = controller.Get().ToList();
        var result2 = controller.Get().ToList();

        // Assert
        // Since Random is used, results should not always be the same
        Assert.False(result1.SequenceEqual(result2));
    }

    [Fact]
    public void Get_ThrowsException_WhenLoggerIsNull()
    {
        // Negative scenario: passing null logger should throw
        Assert.Throws<System.ArgumentNullException>(() =>
        {
            var controller = new WeatherForecastController(null!);
        });
    }
   
    // ... (other test cases)

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void GetById_ValidId_ReturnsWeatherForecast(int id)
    {
        var logger = new LoggerFactory().CreateLogger<WeatherForecastController>();
        var controller = new WeatherForecastController(logger);

        var result = controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var forecast = Assert.IsType<WeatherForecast>(okResult.Value);
        Assert.NotNull(forecast.Summary);
        Assert.InRange(forecast.TemperatureC, -20, 54);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void GetById_InvalidId_ReturnsNotFound(int id)
    {
        var logger = new LoggerFactory().CreateLogger<WeatherForecastController>();
        var controller = new WeatherForecastController(logger);

        var result = controller.GetById(id);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void GetById_ReturnsDifferentForecastsForDifferentIds()
    {
        var logger = new LoggerFactory().CreateLogger<WeatherForecastController>();
        var controller = new WeatherForecastController(logger);

        var result1 = controller.GetById(1).Result as OkObjectResult;
        var result2 = controller.GetById(2).Result as OkObjectResult;

        Assert.NotNull(result1);
        Assert.NotNull(result2);

        var forecast1 = result1.Value as WeatherForecast;
        var forecast2 = result2.Value as WeatherForecast;

        Assert.NotNull(forecast1);
        Assert.NotNull(forecast2);

        // Dates should be different for different ids
        Assert.NotEqual(forecast1.Date, forecast2.Date);
    }

    [Fact]
    public void GetById_LoggerIsNull_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var controller = new WeatherForecastController(null!);
            controller.GetById(1);
        });
    }

    [Fact]
    public void GetById_SummaryIsFromPredefinedList()
    {
        var logger = new LoggerFactory().CreateLogger<WeatherForecastController>();
        var controller = new WeatherForecastController(logger);

        var result = controller.GetById(2);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var forecast = Assert.IsType<WeatherForecast>(okResult.Value);

        var summaries = new[]
        {
            "Kishan.Yogarajan", "Sujan.Yogarajan", "Lingesh.Jawahar", "Soundhar", "Bharathi", "Vishant", "Santhosh", "Yogarajan", "Yogaraja", "Yogaraj"
        };

        Assert.Contains(forecast.Summary, summaries);
    }
}

