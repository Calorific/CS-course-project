using System;
using System.Text.Json.Serialization;

namespace CS_course_project.Model.Timetables; 

public class Lesson : ILesson {
    public string Subject { get; }
    public string Classroom { get; }
    public string Teacher { get; }
    
    
    [JsonConstructor]
    public Lesson(string subject, string classroom, string teacher) {
        if (string.IsNullOrEmpty(subject))
            throw new ArgumentException("Некорректное значение предмеат");
        if (string.IsNullOrEmpty(classroom))
            throw new ArgumentException("Некорректное значение аудитории");
        if (string.IsNullOrEmpty(teacher))
            throw new ArgumentException("Некорректное значение преподавателя");
        
        Subject = subject;
        Classroom = classroom;
        Teacher = teacher;
    }
}