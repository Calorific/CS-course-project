using CS_course_project.Model.Timetables;

namespace CS_course_project.Tests.Model.Entities; 

public class LessonTests {
    [Fact]
    public void ShouldThrowErrorForEmptySubject() {
        // Arrange
        const string subject = "";
        const string classroom = "classroom";
        const string teacher = "teacher";
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Lesson(subject, classroom, teacher));
    }
    
    [Fact]
    public void ShouldThrowErrorForEmptyClassroom() {
        // Arrange
        const string subject = "subject";
        const string classroom = "";
        const string teacher = "teacher";
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Lesson(subject, classroom, teacher));
    }
    
    [Fact]
    public void ShouldThrowErrorForEmptyTeacher() {
        // Arrange
        const string subject = "subject";
        const string classroom = "classroom";
        const string teacher = "";
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Lesson(subject, classroom, teacher));
    }
}