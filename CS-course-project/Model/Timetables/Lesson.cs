using System.Text.Json.Serialization;

namespace CS_course_project.Model.Timetables; 

public class Lesson : ILesson {
    public string Subject { get; set; } = string.Empty;
    public string Classroom { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;

    public Lesson() {}
    
    [JsonConstructor]
    public Lesson(string subject, string classroom, string teacher) {
        Subject = subject;
        Classroom = classroom;
        Teacher = teacher;
    }
}