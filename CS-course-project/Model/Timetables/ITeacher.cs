using System;

namespace CS_course_project.Model.Timetables; 

public interface ITeacher {
    public string Name { get; }
    public Guid Id { get; }
}