using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Timetables;

namespace CS_course_project.Tests.Model.Storage; 

public class MockTeacher : ITeacher {
    public string Name { get; } = "Name";
    public string Id { get; } = "Id";

    public MockTeacher() {}
    
    public MockTeacher(string name, string id) {
        Name = name;
        Id = id;
    }
}

public class MockSettings : ISettings {
    public string HashedAdminPassword { get; } = BCrypt.Net.BCrypt.HashPassword("123");
    public int LessonDuration { get; } = 40;
    public int BreakDuration { get; } = 10;
    public int LongBreakDuration { get; } = 15;
    public int StartTime { get; } = 480;
    public int LessonsNumber { get; } = 4;
    public List<int> LongBreakLessons { get; } = new();
    
    public MockSettings() {}

    public MockSettings(string hashedAdminPassword) {
        HashedAdminPassword = hashedAdminPassword;
    }
}

public class MockSession : ISession {
    public bool IsAdmin { get; }
    public string Data { get; }

    public MockSession(bool isAdmin, string data) {
        IsAdmin = isAdmin;
        Data = data;
    }
}

public class MockTimetable : ITimetable {
    public string Group { get; }
    public IList<IDay> Days { get; }

    public MockTimetable(string group, IList<IDay> days) {
        Group = group;
        Days = days;
    }
}

public class MockDay : IDay {
    public IList<ILesson?> Lessons { get; }

    public MockDay(IList<ILesson?> lessons) {
        Lessons = lessons;
    }
}

public class MockLesson : ILesson {
    public string Subject { get; }
    public string Classroom { get; }
    public string Teacher { get; }

    public MockLesson(string subject, string classroom, string teacher) {
        Subject = subject;
        Classroom = classroom;
        Teacher = teacher;
    }
}