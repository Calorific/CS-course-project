using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Storage;
using CS_course_project.Model.Timetables;
using CS_course_project.Tests.Model.Storage;

namespace CS_course_project.Tests.ViewModel.Components.TimetableForm;

public class MockNavigator : INavigator {
    public void Navigate(Routes pageName) {
        throw new NotImplementedException();
    }
}

public class MockDataManager : IDataManager {
    public Task<Dictionary<string, ITimetable>> GetTimetables() {
        var lesson = new MockLesson("subject", "classroom", "teacher");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("group", new List<IDay> { day });
        return Task.Run(() => new Dictionary<string, ITimetable> { { "group", timetable } });
    }

    public Task<bool> AddTimetable(ITimetable newTimetable) {
        throw new NotImplementedException();
    }

    public Task<ISession?> GetSession() {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateSession(ISession newItem) {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveSession() {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetGroups() {
        return Task.Run(() => new List<string> { "group", "group2" });
    }

    public Task<bool> UpdateGroups(string newItem) {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveGroup(string item) {
        throw new NotImplementedException();
    }

    public Task<List<ITeacher>> GetTeachers() {
        return Task.Run(() => new List<ITeacher> { new Teacher("teacher1"), new Teacher("teacher2") });
    }

    public Task<bool> UpdateTeachers(ITeacher newItem) {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveTeacher(string id) {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetClassrooms() {
        return Task.Run(() => new List<string> { "classroom", "classroom2" });
    }

    public Task<bool> UpdateClassrooms(string newClassroom) {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveClassroom(string item) {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetSubjects() {
        return Task.Run(() => new List<string> { "subject", "subject2" });
    }

    public Task<bool> UpdateSubjects(string newSubject) {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveSubject(string item) {
        throw new NotImplementedException();
    }

    public Task<ISettings> GetSettings() {
        return Task.Run(() => new MockSettings() as ISettings);
    }

    public Task UpdateSettings(ISettings settings) {
        throw new NotImplementedException();
    }
}