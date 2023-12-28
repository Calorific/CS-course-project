
using CS_course_project.Model.Timetables;

namespace CS_course_project.Tests.Model.Entities; 

public class SettingsTests {
    private const string HashedAdminPassword = "123";
    private const int LessonDuration = 40;
    private const int BreakDuration = 10;
    private const int LongBreakDuration = 15;
    private const int StartTime = 480;
    private const int LessonsNumber = 4;
    private readonly List<int> _longBreakLessons = new();
    
    [Fact]
    public void ShouldThrowErrorForInvalidLessonDuration() {
        // Arrange
        const int bigLessonDuration = 10000;
        const int negativeLessonDuration = -1;
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Settings(bigLessonDuration, BreakDuration, LongBreakDuration,
            StartTime, HashedAdminPassword, LessonsNumber, _longBreakLessons));
        Assert.Throws<ArgumentException>(() => new Settings(negativeLessonDuration, BreakDuration, LongBreakDuration,
            StartTime, HashedAdminPassword, LessonsNumber, _longBreakLessons));
    }
    
    [Fact]
    public void ShouldThrowErrorForInvalidBreakDuration() {
        // Arrange
        const int bigBreakDuration = 10000;
        const int negativeBreakDuration = -1;
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Settings(LessonDuration, bigBreakDuration, LongBreakDuration,
            StartTime, HashedAdminPassword, LessonsNumber, _longBreakLessons));
        Assert.Throws<ArgumentException>(() => new Settings(LessonDuration, negativeBreakDuration, LongBreakDuration,
            StartTime, HashedAdminPassword, LessonsNumber, _longBreakLessons));
    }
    
    [Fact]
    public void ShouldThrowErrorForInvalidLongBreakDuration() {
        // Arrange
        const int bigLongBreakDuration = 10000;
        const int negativeLongBreakDuration = -1;
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Settings(LessonDuration, BreakDuration, bigLongBreakDuration,
            StartTime, HashedAdminPassword, LessonsNumber, _longBreakLessons));
        Assert.Throws<ArgumentException>(() => new Settings(LessonDuration, BreakDuration, negativeLongBreakDuration,
            StartTime, HashedAdminPassword, LessonsNumber, _longBreakLessons));
    }
    
    [Fact]
    public void ShouldThrowErrorForInvalidStartTime() {
        // Arrange
        const int bigStartTime = 10000;
        const int negativeStartTime = -1;
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Settings(LessonDuration, BreakDuration, LongBreakDuration,
            bigStartTime, HashedAdminPassword, LessonsNumber, _longBreakLessons));
        Assert.Throws<ArgumentException>(() => new Settings(LessonDuration, BreakDuration, LongBreakDuration,
            negativeStartTime, HashedAdminPassword, LessonsNumber, _longBreakLessons));
    }
    
    [Fact]
    public void ShouldThrowErrorForEmptyAdminPassword() {
        // Arrange
        const string hashedAdminPassword = "";
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Settings(LessonDuration, BreakDuration, LongBreakDuration,
            StartTime, hashedAdminPassword, LessonsNumber, _longBreakLessons));
    }
    
    [Fact]
    public void ShouldThrowErrorForInvalidLessonNumber() {
        // Arrange
        const int bigLessonsNumber = 100;
        const int negativeLessonsNumber = -1;
        
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Settings(LessonDuration, BreakDuration, LongBreakDuration,
            StartTime, HashedAdminPassword, bigLessonsNumber, _longBreakLessons));
        Assert.Throws<ArgumentException>(() => new Settings(LessonDuration, BreakDuration, LongBreakDuration,
            StartTime, HashedAdminPassword, negativeLessonsNumber, _longBreakLessons));
    }
    
    [Fact]
    public void ShouldThrowErrorForInvalidLongBreakLessons() {
        // Arrange
        var longBrakes = new List<int> { LessonsNumber };
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Settings(LessonDuration, BreakDuration, LongBreakDuration,
            StartTime, HashedAdminPassword, LessonsNumber, longBrakes));
    }
    
    [Fact]
    public void ShouldThrowErrorWhenTimesExceedsDay() {
        // Arrange
        const int lessonsNumber = 90;
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Settings(LessonDuration, BreakDuration, LongBreakDuration,
            StartTime, HashedAdminPassword, lessonsNumber, _longBreakLessons));
    }
}