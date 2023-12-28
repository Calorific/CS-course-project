using CS_course_project.Model.Timetables;
using System.Text.Json;
using CS_course_project.Model.Storage;

namespace CS_course_project.Tests.Model.Storage;

public class TimetablesReporitoryTests {
    private const string Path = "./data/timetables.json";
    [Fact]
    public void ShouldCreateFileWhenInstanciated() {
        if (File.Exists(Path))
            File.Delete(Path);
        
        
        // Arrange & Act
        const bool expected = true;
        
        _ = new TimetablesRepository();
        
        
        // Assert
        Assert.Equal(expected, File.Exists(Path));
    }

    [Fact]
    public async void ShouldCreateData() {
        if (File.Exists(Path))
            await File.WriteAllTextAsync(Path, "");
        
        // Arrange
        var lesson = new MockLesson("subject", "classroom", "teacher");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("group", new List<IDay> { day });
        var expected = JsonSerializer.Serialize(new Dictionary<string, ITimetable> { { "group", timetable } });
        
        var timetablesRepository = new TimetablesRepository();
        
        
        // Act
        await timetablesRepository.Update(timetable);

        
        // Assert
        Assert.Equal(expected, await File.ReadAllTextAsync(Path));
    }
    
    [Fact]
    public async void ShouldUpdateData() {
        if (File.Exists(Path))
            await File.WriteAllTextAsync(Path, "");
        
        // Arrange
        var lesson = new MockLesson("subject", "classroom", "teacher");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("group", new List<IDay> { day });
        var timetable2 = new MockTimetable("group2", new List<IDay> { day });
        var expected = JsonSerializer.Serialize(new Dictionary<string, ITimetable> { { "group", timetable },
            { "group2", timetable2 } });
        
        var timetablesRepository = new TimetablesRepository();
        
        
        // Act
        await timetablesRepository.Update(timetable);
        await timetablesRepository.Update(timetable2);

        
        // Assert
        Assert.Equal(expected, await File.ReadAllTextAsync(Path));
    }
    
    [Fact]
    public async void ShouldRemoveData() {
        if (File.Exists(Path))
            await File.WriteAllTextAsync(Path, "");
        
        // Arrange
        var lesson = new MockLesson("subject", "classroom", "teacher");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("group", new List<IDay> { day });
        const string expected = "{}";
        
        var timetablesRepository = new TimetablesRepository();
        
        
        // Act
        await timetablesRepository.Update(timetable);
        await timetablesRepository.Delete("group");
        
        // Assert
        Assert.Equal(expected, await File.ReadAllTextAsync(Path));
    }
}