using System.Collections.Generic;

namespace CS_course_project.Model.Timetables; 

public interface ITimetable {
    public string Group { get; }
    public IList<IDay> Days { get; }
}