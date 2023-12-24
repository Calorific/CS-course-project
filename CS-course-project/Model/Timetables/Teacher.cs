using System;
using System.Text.Json.Serialization;

namespace CS_course_project.Model.Timetables; 

public class Teacher : ITeacher {
    public string Name { get; }
    public string Id { get; }

    public Teacher(string name) {
        Name = name;
        Id = Guid.NewGuid().ToString();
    }
    
    [JsonConstructor]
    public Teacher(string name, string id) {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Имя преподавателя не может быть пустым");
        if (string.IsNullOrEmpty(id))
            throw new ArgumentException("ID преподавателя не может быть пустым");
        Name = name;
        Id = id;
    }
}