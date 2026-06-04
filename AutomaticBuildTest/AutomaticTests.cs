using Xunit;
using Microsoft.Extensions.Logging;
using AutomaticBuild.Controllers;
using System.Collections.Generic;
using System.Linq;

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
            var controller = new WeatherForecastController(null);
        });
    }
}
