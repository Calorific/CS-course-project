using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Storage;
using CS_course_project.Model.Timetables;

namespace CS_course_project.Tests.ViewModel.Pages.LoginViewModelPage;

public class MockNavigator : INavigator {
    public void Navigate(Routes pageName) {
        throw new NotImplementedException();
    }
}

public class MockAuthService : IAuthService {
    public Task<string?> LogIn(string data, bool isAdmin) {
        throw new NotImplementedException();
    }

    public Task<bool> LogOut() {
        throw new NotImplementedException();
    }
}

public class MockDataManager : IDataManager {
    private readonly Dictionary<string, ITimetable> _timetables = new();
    public Task<Dictionary<string, ITimetable>> GetTimetables() {
        return Task.Run(() => _timetables);
    }

    public Task<bool> AddTimetable(ITimetable newTimetable) {
        return Task.Run(() => {
            _timetables[newTimetable.Group] = newTimetable;
            return true;
        });
    }

    public Task<ISession?> GetSession() {
        return Task.Run(() => null as ISession);
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
        throw new NotImplementedException();
    }

    public Task UpdateSettings(ISettings settings) {
        throw new NotImplementedException();
    }
}