
using System.Text.Json.Serialization;

namespace CS_course_project.Model.Timetables; 

[JsonConverter(typeof(Lesson))]
public interface ILesson {
    public string Subject { get; }
    public string Classroom { get; }
    public string Teacher { get; }
}