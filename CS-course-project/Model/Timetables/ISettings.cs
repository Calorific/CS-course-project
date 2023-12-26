using System.Collections.Generic;

namespace CS_course_project.Model.Timetables; 

public interface ISettings {
    public string HashedAdminPassword { get; }

    public int LessonDuration { get; }
    
    public int BreakDuration { get; }
    
    public int LongBreakDuration { get; }
    
    public int StartTime { get; }
    
    public int LessonsNumber { get; }

    public List<int> LongBreakLessons { get; }
}