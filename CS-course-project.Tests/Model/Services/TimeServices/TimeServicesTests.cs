using CS_course_project.Model.Services.TimeServices;

namespace CS_course_project.Tests.Model.Services.TimeServices; 

public class TimeServicesTests {
    [Fact]
    public void ShouldParseTimeCorrectly() {
        // Arrange
        var converter = new TimeConverter();
        const string time = "8:30";
        const int expected = 510;
        
        
        // Act
        var actual = converter.ParseTime(time);
        
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void ShouldThrowErrorForEmptyInput() {
        // Arrange
        var converter = new TimeConverter();
        const string time = "";
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => converter.ParseTime(time));
    }
    
    [Fact]
    public void ShouldFormatTimeCorrectly() {
        // Arrange
        var converter = new TimeConverter();
        const int time = 510;
        const string expected = "8:30";
        
        
        // Act
        var actual = converter.FormatTime(time);
        
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void ShouldThrowErrorForNegativeTime() {
        // Arrange
        var converter = new TimeConverter();
        const int time = -1;
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => converter.FormatTime(time));
    }
}