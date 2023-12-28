using CS_course_project.Model.Timetables;
using CS_course_project.Tests.Model.Storage;

namespace CS_course_project.Tests.Model.Entities; 

public class TimetableTests {
    [Fact]
    public void ShouldThrowErrorForEmptyDaysList() {
        // Arrange
        var days = new List<IDay>();
        const string group = "group";
        
        
        // Act & Arrange
        Assert.Throws<ArgumentException>(() => new Timetable(group, days));
    }
    
    [Fact]
    public void ShouldThrowErrorForEmptyGroup() {
        // Arrange
        var days = new List<IDay> { new MockDay(new List<ILesson?>()) };
        const string group = "";
        
        
        // Act & Arrange
        Assert.Throws<ArgumentException>(() => new Timetable(group, days));
    }
}