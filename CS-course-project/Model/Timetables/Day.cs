using System.Collections.Generic;

namespace CS_course_project.Model.Timetables; 

public class Day : IDay {
    public IList<ILesson?> Lessons { get; }
    
    public Day(IList<ILesson?> lessons) {
        Lessons = lessons;
    }
}