using CS_course_project.ViewModel.Components.TimetableForm;

namespace CS_course_project.Tests.ViewModel.Components.TimetableForm; 

public class LessonViewModelTests {
    [Fact]
    public void ShouldAddErrorsToEmptyFieldsWhenClassroomNotEmpty() {
        // Arrange
        var lessonViewModel = new LessonViewModel(0, 0, (_, _) => null, (_, _) => null, (_, _) => null);
        const int expected = 2;
        
        // Act
        lessonViewModel.Classroom = "classroom";
        
        
        // Assert
        Assert.Equal(expected, lessonViewModel.Errors.Count);
    }
    
    [Fact]
    public void ShouldAddErrorsToEmptyFieldsWhenSubjectNotEmpty() {
        // Arrange
        var lessonViewModel = new LessonViewModel(0, 0, (_, _) => null, (_, _) => null, (_, _) => null);
        const int expected = 2;
        
        // Act
        lessonViewModel.Subject = "subject";
        
        
        // Assert
        Assert.Equal(expected, lessonViewModel.Errors.Count);
    }
    
    [Fact]
    public void ShouldAddErrorsToEmptyFieldsWhenTeacherNotEmpty() {
        // Arrange
        var lessonViewModel = new LessonViewModel(0, 0, (_, _) => null, (_, _) => null, (_, _) => null);
        const int expected = 2;
        
        // Act
        lessonViewModel.Teacher = "teacher";
        
        
        // Assert
        Assert.Equal(expected, lessonViewModel.Errors.Count);
    }
    
    [Fact]
    public void ShouldClearErrorsWhenFieldIsFilled() {
        // Arrange
        var lessonViewModel = new LessonViewModel(0, 0, (_, _) => null, (_, _) => null, (_, _) => null)
            {
                // Act
                Teacher = "teacher",
                Classroom = "classroom"
            };


        // Assert
        Assert.Single(lessonViewModel.Errors);
    }
    
    [Fact]
    public void ShouldClearErrorsWhenEverythingEmpty() {
        // Arrange
        var lessonViewModel = new LessonViewModel(0, 0, (_, _) => null, (_, _) => null, (_, _) => null)
        {
            // Act
            Teacher = "teacher",
            Classroom = "classroom"
        };
        

        // Act
        lessonViewModel.Teacher = "";
        lessonViewModel.Classroom = "";
        

        // Assert
        Assert.Empty(lessonViewModel.Errors);
    }
    
    [Fact]
    public void ShouldBeNoErrorsWhenEverythingIsFilled() {
        // Arrange
        var lessonViewModel = new LessonViewModel(0, 0, (_, _) => null, (_, _) => null, (_, _) => null)
        {
            // Act
            Teacher = "teacher",
            Classroom = "classroom",
            Subject = "subject"
        };
        

        // Assert
        Assert.Empty(lessonViewModel.Errors);
    }
    
    [Fact]
    public void ShoulAddErrorsFromCallback() {
        // Arrange
        var lessonViewModel = new LessonViewModel(0, 0, (_, _) => null, (_, _) => "error", (_, _) => null)
        {
            // Act
            Teacher = "teacher",
            Classroom = "classroom",
            Subject = "subject"
        };
        

        // Assert
        Assert.Single(lessonViewModel.Errors);
    }
}