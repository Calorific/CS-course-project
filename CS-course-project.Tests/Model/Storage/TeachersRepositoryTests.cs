using CS_course_project.Model.Storage;
using System.Text.Json;

namespace CS_course_project.Tests.Model.Storage;

public class TeachersRepositoryTests {
    private const string Path = "./data/teachers.json";
    [Fact]
    public void ShouldCreateFileWhenInstanciated() {
        if (File.Exists(Path))
            File.Delete(Path);
        
        
        // Arrange & Act
        const bool expected = true;
        
        _ = new TeachersRepository();
        
        
        // Assert
        Assert.Equal(expected, File.Exists(Path));
    }

    [Fact]
    public async void ShouldCreateData() {
        if (File.Exists(Path))
            await File.WriteAllTextAsync(Path, "");
        
        // Arrange
        var expected = JsonSerializer.Serialize(new List<MockTeacher> { new() });
        
        var teachersRepository = new TeachersRepository();
        
        
        // Act
        await teachersRepository.Update(new MockTeacher());

        
        // Assert
        Assert.Equal(expected, await File.ReadAllTextAsync(Path));
    }
    
    [Fact]
    public async void ShouldUpdateData() {
        if (File.Exists(Path))
            await File.WriteAllTextAsync(Path, "");
        
        // Arrange
        var expected = JsonSerializer.Serialize(new List<MockTeacher> { new(), new() });
        
        var teachersRepository = new TeachersRepository();
        
        
        // Act
        await teachersRepository.Update(new MockTeacher());
        await teachersRepository.Update(new MockTeacher());

        
        // Assert
        Assert.Equal(expected, await File.ReadAllTextAsync(Path));
    }
    
    [Fact]
    public async void ShouldRemoveData() {
        if (File.Exists(Path))
            await File.WriteAllTextAsync(Path, "");
        
        // Arrange
        const string expected = "[]";
        
        var teachersRepository = new TeachersRepository();
        
        
        // Act
        await teachersRepository.Update(new MockTeacher());
        await teachersRepository.Delete("Id");
        
        // Assert
        Assert.Equal(expected, await File.ReadAllTextAsync(Path));
    }
}