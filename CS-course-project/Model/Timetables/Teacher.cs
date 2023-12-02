﻿using System;

namespace CS_course_project.Model.Timetables; 

public class Teacher : ITeacher {
    public string Name { get; }
    public Guid Id { get; }

    public Teacher(string name) {
        Name = name;
        Id = Guid.NewGuid();
    }
}