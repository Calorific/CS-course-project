namespace CS_course_project.Model.Timetables; 

public interface ILesson {
    public string Subject { get; }
    public string Classroom { get; }
    public string Teacher { get; }
}