using CS_course_project.Model.Storage;
using System.Text.Json;

namespace CS_course_project.Tests.Model.Storage;

public class SessionRepositoryTests {
    private const string Path = "./data/session.json";
    
    [Fact]
    public void ShouldCreateFileWhenInstanciated() {
        if (File.Exists(Path))
            File.Delete(Path);
        
        
        // Arrange & Act
        const bool expected = true;
        
        _ = new SessionRepository();
        
        
        // Assert
        Assert.Equal(expected, File.Exists(Path));
    }

    [Fact]
    public async void ShouldCreateData() {
        if (File.Exists(Path))
            await File.WriteAllTextAsync(Path, "");
        
        // Arrange
        var expected = JsonSerializer.Serialize(new MockSession(false, "АВТ-213"));
        
        var sessionRepository = new SessionRepository();
        
        
        // Act
        await sessionRepository.Update(new MockSession(false, "АВТ-213"));

        
        // Assert
        Assert.Equal(expected, await File.ReadAllTextAsync(Path));
    }
    
    [Fact]
    public async void ShouldRemoveData() {
        if (File.Exists(Path))
            await File.WriteAllTextAsync(Path, "");
        
        // Arrange
        const string expected = "";
        
        var sessionRepository = new SessionRepository();
        
        
        // Act
        await sessionRepository.Update(new MockSession(false, "АВТ-213"));
        await sessionRepository.Delete(true);
        
        // Assert
        Assert.Equal(expected, await File.ReadAllTextAsync(Path));
    }
}