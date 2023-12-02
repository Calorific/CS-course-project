namespace CS_course_project.Model.Timetables; 

public interface ILesson {
    public string Subject { get; set; }
    public string Classroom { get; set; }
    public string Teacher { get; set; }
}