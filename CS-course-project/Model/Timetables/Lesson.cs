using System.Text.Json.Serialization;

namespace CS_course_project.Model.Timetables; 

public class Lesson {
    public string Subject { get; } = string.Empty;
    public string Classroom { get; } = string.Empty;
    public string Teacher { get; } = string.Empty;

    public Lesson() {}
    
    [JsonConstructor]
    public Lesson(string subject, string classroom, string teacher) {
        Subject = subject;
        Classroom = classroom;
        Teacher = teacher;
    }
}