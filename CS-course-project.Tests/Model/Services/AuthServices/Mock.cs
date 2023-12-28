using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Storage;
using CS_course_project.Model.Timetables;
using CS_course_project.Tests.Model.Storage;

namespace CS_course_project.Tests.Model.Services.AuthServices;

public class MockAuthDataManager : IDataManager {
    public Task<Dictionary<string, ITimetable>> GetTimetables() {
        return Task.Run(() => new Dictionary<string, ITimetable> { { "group", null } });
    }

    public Task<bool> AddTimetable(ITimetable newTimetable) {
        throw new NotImplementedException();
    }

    public Task<ISession?> GetSession() {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateSession(ISession newItem) {
        return Task.Run(() => true);
    }

    public Task<bool> RemoveSession() {
        return Task.Run(() => true);
    }

    public Task<List<string>> GetGroups() {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateGroups(string newItem) {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveGroup(string item) {
        throw new NotImplementedException();
    }

    public Task<List<ITeacher>> GetTeachers() {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateTeachers(ITeacher newItem) {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveTeacher(string id) {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetClassrooms() {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateClassrooms(string newClassroom) {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveClassroom(string item) {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetSubjects() {
        throw new NotImplementedException();
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