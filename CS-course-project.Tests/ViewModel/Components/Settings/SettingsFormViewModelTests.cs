using CS_course_project.ViewModel.Components.Settings;

namespace CS_course_project.Tests.ViewModel.Components.Settings; 

public class SettingsFormViewModelTests {
    [Fact]
    public void ShouldAddErrorForEmptyLessonDuration() {
        // Arrange
        var viewModel = new SettingsFormViewModel {
            // Act
            LessonDuration = string.Empty
        };
        
        
        // Assert
        Assert.Single(viewModel.Errors);
    }
    
    [Fact]
    public void ShouldAddErrorForInvalidLessonDuration() {
        // Arrange
        var viewModel = new SettingsFormViewModel {
            // Act
            LessonDuration = "0"
        };
        
        
        // Assert
        Assert.Single(viewModel.Errors);
    }
    
    [Fact]
    public void ShouldAddErrorForEmptyBreakDuration() {
        // Arrange
        var viewModel = new SettingsFormViewModel {
            // Act
            BreakDuration = ""
        };
        
        
        // Assert
        Assert.Single(viewModel.Errors);
    }
    
    [Fact]
    public void ShouldAddErrorForEmptyLongBreakDuration() {
        // Arrange
        var viewModel = new SettingsFormViewModel {
            // Act
            LongBreakDuration = ""
        };
        
        
        // Assert
        Assert.Single(viewModel.Errors);
    }
    
    [Fact]
    public void ShouldAddErrorForEmptyLessonsNumber() {
        // Arrange
        var viewModel = new SettingsFormViewModel {
            // Act
            BreakDuration = ""
        };
        
        
        // Assert
        Assert.Single(viewModel.Errors);
    }
    
    [Fact]
    public void ShouldAddErrorForInvalidLessonsNumber() {
        // Arrange
        var viewModel = new SettingsFormViewModel {
            LessonsNumber = "10"
        };

        
        // Act
        viewModel.LessonsNumber = "0";
        
        
        // Assert
        Assert.Single(viewModel.Errors);
    }
    
    [Fact]
    public void ShouldAddErrorWhenExceedingTime() {
        // Arrange
        var viewModel = new SettingsFormViewModel(new MockDataManager(), new MockTimeConverter()) {
            StartTime = "8:30",
            BreakDuration = "10",
            LongBreakDuration = "15",
            LessonDuration = "120",
            LessonsNumber = "4"
        };
        
        
        // Act
        viewModel.LessonsNumber = "15";
        
        // Assert
        Assert.Single(viewModel.Errors);
    }
}