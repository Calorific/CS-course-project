using CS_course_project.Model.Storage;
using System.Text.Json;

namespace CS_course_project.Tests.Model.Storage; 

public class SettingsRepositoryTests {
    private const string Path = "./data/settings.json";
    [Fact]
    public void ShouldCreateFileWhenInstanciated() {
        if (File.Exists(Path))
            File.Delete(Path);
        
        
        // Arrange & Act
        const bool expected = true;
        
        _ = new SettingsRepository();
        
        
        // Assert
        Assert.Equal(expected, File.Exists(Path));
    }

    [Fact]
    public async void ShouldCreateData() {
        if (File.Exists(Path))
            await File.WriteAllTextAsync(Path, "");
        
        // Arrange
        var expected = JsonSerializer.Serialize(new MockSettings("123"));
        
        var settingsRepository = new SettingsRepository();
        
        
        // Act
        await settingsRepository.Update(new MockSettings("123"));

        
        // Assert
        Assert.Equal(expected, await File.ReadAllTextAsync(Path));
    }
    
    [Fact]
    public async void ShouldRemoveData() {
        if (File.Exists(Path))
            await File.WriteAllTextAsync(Path, "");
        
        // Arrange
        const string expected = "";
        
        var settingsRepository = new SettingsRepository();
        
        
        // Act
        await settingsRepository.Update(new MockSettings());
        await settingsRepository.Delete(true);
        
        // Assert
        Assert.Equal(expected, await File.ReadAllTextAsync(Path));
    }
}