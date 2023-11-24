using System;

namespace CS_course_project.Model.Timetables; 

public class Teacher {
    public string Name { get; set; }
    public Guid Id { get; set; }

    public Teacher(string name) {
        Name = name;
        Id = Guid.NewGuid();
    }
}