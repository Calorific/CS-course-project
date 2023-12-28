using CS_course_project.Model.Timetables;

namespace CS_course_project.Tests.Model.Entities; 

public class DayTests {
    [Fact]
    public void ShouldThrowErrorForEmptyLessonsList() {
        // Arrange
        var lessons = new List<ILesson?>() as IList<ILesson?>;
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Day(lessons));
    }
}