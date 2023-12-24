using System;
using System.Collections.Generic;

namespace CS_course_project.Model.Timetables; 

public class Day : IDay {
    public IList<ILesson?> Lessons { get; }
    
    public Day(IList<ILesson?> lessons) {
        if (lessons == null || lessons.Count == 0)
            throw new ArgumentException("Список уроков не может быть пустым");
        Lessons = lessons;
    }
}