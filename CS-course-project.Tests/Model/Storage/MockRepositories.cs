using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Storage;
using CS_course_project.Model.Timetables;

namespace CS_course_project.Tests.Model.Storage; 

public class MockBaseRepository : IRepository<string,List<string>,string> {
    private readonly List<string> _data;
    public Task<bool> Update(string newItem) {
        return Task.Run(() => {
            _data.Add(newItem);
            return true;
        });
    }

    public Task<List<string>> Read() {
        return Task.Run(() => _data);
    }

    public Task<bool> Delete(string key) {
        return Task.Run(() => {
            _data.Remove(key);
            return true;
        });
    }

    public MockBaseRepository(List<string> data) {
        _data = data;
    }
}

public class MockTeachersRepository : IRepository<ITeacher, List<ITeacher>, string> {
    private readonly List<ITeacher> _data = new List<ITeacher>() { new MockTeacher("teacher", "id") };
    public Task<bool> Update(ITeacher newItem) {
        return Task.Run(() => {
            _data.Add(newItem);
            return true;
        });
    }

    public Task<List<ITeacher>> Read() {
        return Task.Run(() => {
            _data.Add(new MockTeacher("test", "testId"));
            return _data;
        });
    }

    public Task<bool> Delete(string key) {
        throw new NotImplementedException();
    }
} 

public class MockSettingsRepository : IRepository<ISettings, ISettings, bool> {
    public Task<bool> Update(ISettings newItem) {
        throw new NotImplementedException();
    }

    public Task<ISettings> Read() {
        return Task.Run(() => new MockSettings() as ISettings);
    }

    public Task<bool> Delete(bool key) {
        throw new NotImplementedException();
    }
}

public class MockTimetablesRepository : IRepository<ITimetable, Dictionary<string, ITimetable>, string> {
    private readonly Dictionary<string, ITimetable> _data;
    
    public Task<bool> Update(ITimetable newItem) {
        return Task.Run(() => {
            _data[newItem.Group] = newItem;
            return true;
        });
    }

    public Task<Dictionary<string, ITimetable>> Read() {
        return Task.Run(() => _data);
    }

    public Task<bool> Delete(string key) {
        return Task.Run(() => _data.Remove(key));
    }

    public MockTimetablesRepository() {
        var lesson = new MockLesson("subject", "classroom", "id");
        var day = new MockDay(new List<ILesson?> { lesson });
        var timetable = new MockTimetable("group", new List<IDay> { day });
        _data = new Dictionary<string, ITimetable> { { "group", timetable } };
    }
}

public class MockSessionRepository : IRepository<ISession, ISession?, bool> {
    public Task<bool> Update(ISession newItem) {
        throw new NotImplementedException();
    }

    public Task<ISession?> Read() {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(bool key) {
        throw new NotImplementedException();
    }
}