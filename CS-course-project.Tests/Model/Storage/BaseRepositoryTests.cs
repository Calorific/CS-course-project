using CS_course_project.Model.Storage;

namespace CS_course_project.Tests.Model.Storage;

public class BaseRepositoryTests {
    
    [Fact]
    public void ShouldCreateFileWhenInstanciated() {
        if (File.Exists("./data/groups.json"))
            File.Delete("./data/groups.json");
        if (File.Exists("./data/classrooms.json"))
            File.Delete("./data/classrooms.json");
        if (File.Exists("./data/subjects.json"))
            File.Delete("./data/subjects.json");
        
        // Arrange & Act
        const bool expected = true;
        
        _ = new BaseRepository(RepositoryItems.Groups);
        _ = new BaseRepository(RepositoryItems.Classrooms);
        _ = new BaseRepository(RepositoryItems.Subjects);
        
        
        // Assert
        Assert.Equal(expected, File.Exists("./data/groups.json"));
        Assert.Equal(expected, File.Exists("./data/classrooms.json"));
        Assert.Equal(expected, File.Exists("./data/subjects.json"));
    }

    [Fact]
    public async void ShouldCreateData() {
        if (File.Exists("./data/groups.json"))
            await File.WriteAllTextAsync("./data/groups.json", "");
        if (File.Exists("./data/classrooms.json"))
            await File.WriteAllTextAsync("./data/classrooms.json", "");
        if (File.Exists("./data/subjects.json"))
            await File.WriteAllTextAsync("./data/subjects.json", "");
        
        // Arrange
        const string expectedGroups = "[\"groupA\"]";
        const string expectedClassrooms = "[\"classroomA\"]";
        const string expectedSubjects = "[\"subjectA\"]";
        
        var groupRepository = new BaseRepository(RepositoryItems.Groups);
        var classroomRepository = new BaseRepository(RepositoryItems.Classrooms);
        var subjectRepository = new BaseRepository(RepositoryItems.Subjects);
        
        
        // Act
        await groupRepository.Update("groupA");
        await classroomRepository.Update("classroomA");
        await subjectRepository.Update("subjectA");

        
        // Assert
        Assert.Equal(expectedGroups, await File.ReadAllTextAsync("./data/groups.json"));
        Assert.Equal(expectedClassrooms, await File.ReadAllTextAsync("./data/classrooms.json"));
        Assert.Equal(expectedSubjects, await File.ReadAllTextAsync("./data/subjects.json"));
    }

    [Fact]
    public async void ShouldUpdateData() {
        if (File.Exists("./data/groups.json"))
            await File.WriteAllTextAsync("./data/groups.json", "");
        if (File.Exists("./data/classrooms.json"))
            await File.WriteAllTextAsync("./data/classrooms.json", "");
        if (File.Exists("./data/subjects.json"))
            await File.WriteAllTextAsync("./data/subjects.json", "");
        
        
        // Arrange
        const string expectedGroups = "[\"groupA\",\"groupB\"]";
        const string expectedClassrooms = "[\"classroomA\",\"classroomB\"]";
        const string expectedSubjects = "[\"subjectA\",\"subjectB\"]";
        
        var groupRepository = new BaseRepository(RepositoryItems.Groups);
        var classroomRepository = new BaseRepository(RepositoryItems.Classrooms);
        var subjectRepository = new BaseRepository(RepositoryItems.Subjects);
        
        
        // Act
        await groupRepository.Update("groupA");
        await groupRepository.Update("groupB");
        await classroomRepository.Update("classroomA");
        await classroomRepository.Update("classroomB");
        await subjectRepository.Update("subjectA");
        await subjectRepository.Update("subjectB");

        
        // Assert
        Assert.Equal(expectedGroups, await File.ReadAllTextAsync("./data/groups.json"));
        Assert.Equal(expectedClassrooms, await File.ReadAllTextAsync("./data/classrooms.json"));
        Assert.Equal(expectedSubjects, await File.ReadAllTextAsync("./data/subjects.json"));
    }
    
    [Fact]
    public async void ShouldRemoveData() {
        if (File.Exists("./data/groups.json"))
            await File.WriteAllTextAsync("./data/groups.json", "");
        if (File.Exists("./data/classrooms.json"))
            await File.WriteAllTextAsync("./data/classrooms.json", "");
        if (File.Exists("./data/subjects.json"))
            await File.WriteAllTextAsync("./data/subjects.json", "");
        
        
        // Arrange
        const string expectedGroups = "[\"groupB\"]";
        const string expectedClassrooms = "[\"classroomA\"]";
        const string expectedSubjects = "[]";
        
        var groupRepository = new BaseRepository(RepositoryItems.Groups);
        var classroomRepository = new BaseRepository(RepositoryItems.Classrooms);
        var subjectRepository = new BaseRepository(RepositoryItems.Subjects);
        
        
        // Act
        await groupRepository.Update("groupA");
        await groupRepository.Update("groupB");
        await classroomRepository.Update("classroomA");
        await classroomRepository.Update("classroomB");
        await subjectRepository.Update("subjectA");
        await subjectRepository.Update("subjectB");

        await groupRepository.Delete("groupA");
        await classroomRepository.Delete("classroomB");
        await subjectRepository.Delete("subjectA");
        await subjectRepository.Delete("subjectB");
        
        
        // Assert
        Assert.Equal(expectedGroups, await File.ReadAllTextAsync("./data/groups.json"));
        Assert.Equal(expectedClassrooms, await File.ReadAllTextAsync("./data/classrooms.json"));
        Assert.Equal(expectedSubjects, await File.ReadAllTextAsync("./data/subjects.json"));
    }
}