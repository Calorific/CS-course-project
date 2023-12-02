using System.Collections.Generic;

namespace CS_course_project.Model.Timetables; 

public interface IDay {
    public IList<ILesson>? Lessons { get; set; }
}