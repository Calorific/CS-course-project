using CS_course_project.Model.Storage;
using CS_course_project.Model.Timetables;
using CS_course_project.Model.Validations;

namespace CS_course_project.Tests.Model.Storage; 

public class DataManagerTests {
    private readonly DataManager _dataManager = new(new ValidationManager(), new MockBaseRepository(new List<string> { "group" }), 
        new MockBaseRepository(new List<string> { "classroom" } ), new MockBaseRepository(new List<string> { "subject" }),
        new MockTeachersRepository(), new MockTimetablesRepository(), new MockSettingsRepository(), new MockSessionRepository());

    #region Timetables

    [Fact]
    public async void ShouldThrowErrorWhenTimetablesConflict() {
        // Arrange
        await _dataManager.UpdateTeachers(new MockTeacher("teacher1", "teacher1"));
        await _dataManager.UpdateClassrooms("classroom1");
        
        var lesson = new MockLesson("subject", "classroom", "teacher1");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("group2", new List<IDay> { day });
        
        var lesson2 = new MockLesson("subject", "classroom1", "id");
        var day2 = new MockDay(new List<ILesson?> { lesson2 });
        var timetable2 = new MockTimetable("group2", new List<IDay> { day2 });
        
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.AddTimetable(timetable));
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.AddTimetable(timetable2));
    }

    #endregion


    #region Groups

    [Fact]
    public async void ShouldThrowErrorWhenGroupIsNotUnique() {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.UpdateGroups("group"));
    }
    
    [Fact]
    public async void ShouldDeleteTimetableForGroup() {
        // Arrange
        const string key = "group";
        
        
        // Act
        await _dataManager.RemoveGroup(key);
        
        
        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () => {
            var timetables = await _dataManager.GetTimetables();
            _ = timetables[key];
        });
    }

    #endregion


    #region Teachers

    [Fact]
    public async void ShouldThrowErrorWhenTeacherIsNotUnique() {
        // Arrange
        await _dataManager.UpdateTeachers(new MockTeacher("name", "id1"));
        
        
        // Act && Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.UpdateTeachers(new MockTeacher("name", "id1")));
    }
    
    [Fact]
    public async void SouldThrowErrorWhenTeacherIsInvalid() {
        // Arrange
        await _dataManager.UpdateSubjects("uniqueSubjectForTeacher");
        await _dataManager.UpdateClassrooms("uniqueClassroomForTeacher");

        
        var lesson = new MockLesson("uniqueSubjectForTeacher", "uniqueClassroomForTeacher", "notExisting");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("groupForClassroom", new List<IDay> { day });
        
        
        // Act && Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.AddTimetable(timetable));
    }

    [Fact]
    public async void ShouldThrowErrorWhenTeacherIsUsed() {
        // Arrange
        await _dataManager.UpdateSubjects("uniqueSubjectForTeacher");
        await _dataManager.UpdateClassrooms("uniqueClassroomForTeacher");
        await _dataManager.UpdateTeachers(new MockTeacher("uniqueTeacherForUsedTest", "uniqueTeacherForUsedTest"));
        
        var lesson = new MockLesson("uniqueSubjectForTeacher", "uniqueClassroomForTeacher", "uniqueTeacherForUsedTest");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("groupForTeacher", new List<IDay> { day });
        await _dataManager.AddTimetable(timetable);
        
        // Act && Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.RemoveTeacher("uniqueTeacherForUsedTest"));
    }

    #endregion


    #region Classrooms

    [Fact]
    public async void ShouldThrowErrorWhenClassroomIsNotUnique() {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.UpdateClassrooms("classroom"));
    }

    [Fact]
    public async void SouldThrowErrorWhenClassroomIsInvalid() {
        // Arrange
        await _dataManager.UpdateSubjects("uniqueSubjectForClassroom");
        await _dataManager.UpdateTeachers(new MockTeacher("name", "uniqueTeacherForClassroom"));

        
        var lesson = new MockLesson("uniqueSubjectForClassroom", "notExisting", "uniqueTeacherForClassroom");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("groupForClassroom", new List<IDay> { day });
       
        
        
        // Act && Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.AddTimetable(timetable));
    }
    
    [Fact]
    public async void ShouldThrowErrorWhenClassroomIsUsed() {
        // Arrange
        await _dataManager.UpdateClassrooms("uniqueClassroom");
        await _dataManager.UpdateTeachers(new MockTeacher("name", "uniqueTeacherForClassroom"));
        await _dataManager.UpdateSubjects("uniqueSubjectForClassroom");
        
        var lesson = new MockLesson("uniqueSubjectForClassroom", "uniqueClassroom", "uniqueTeacherForClassroom");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("groupForClassroom", new List<IDay> { day });
        await _dataManager.AddTimetable(timetable);
        
        
        // Act && Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.RemoveClassroom("uniqueClassroom"));
    }

    #endregion


    #region Subjects

    [Fact]
    public async void ShouldThrowErrorWhenSubjectIsNotUnique() {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.UpdateSubjects("subject"));
    }
    
    [Fact]
    public async void SouldThrowErrorWhenSubjectIsInvalid() {
        // Arrange
        await _dataManager.UpdateClassrooms("uniqueClassroomForSubject");
        await _dataManager.UpdateTeachers(new MockTeacher("t", "uniqueTeacherForSubject"));

        
        var lesson = new MockLesson("notExisting", "uniqueClassroomForSubject", "uniqueTeacherForSubject");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("groupForClassroom", new List<IDay> { day });
        
        
        // Act && Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.AddTimetable(timetable));
    }
    
    [Fact]
    public async void ShouldThrowErrorWhenSubjectIsUsed() {
        // Arrange
        await _dataManager.UpdateSubjects("uniqueSubject");
        await _dataManager.UpdateTeachers(new MockTeacher("t", "uniqueTeacherForSubject"));
        await _dataManager.UpdateClassrooms("uniqueClassroomForSubject");
        
        var lesson = new MockLesson("uniqueSubject", "uniqueClassroomForSubject", "uniqueTeacherForSubject");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("groupForSubject", new List<IDay> { day });
        await _dataManager.AddTimetable(timetable);
        
        // Act && Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _dataManager.RemoveSubject("uniqueSubject"));
    }

    #endregion
}