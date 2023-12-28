using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Services.TimeServices;
using CS_course_project.Model.Storage;
using CS_course_project.Model.Timetables;
using CS_course_project.Tests.Model.Storage;

namespace CS_course_project.Tests.ViewModel.Components.Settings;

public class MockTimeConverter : ITimeConverter {
    public int ParseTime(string time) {
        var parts = time.Split(':');
        var res = 0;
        
        if (int.TryParse(parts[0], out var hours))
            res += hours * 60;
        if (parts.Length > 1 && int.TryParse(parts[1], out var minutes))
            res += minutes;
        return res;
    }

    public string FormatTime(int time) {
        var minutes = time % 60;
        return (time / 60).ToString() + ':' + (minutes < 10 ? "0" : "") + minutes;
    }
}

public class MockDataManager : IDataManager {
    public Task<Dictionary<string, ITimetable>> GetTimetables() {
        throw new NotImplementedException();
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