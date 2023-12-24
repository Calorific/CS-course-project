namespace CS_course_project.Model.Services.TimeServices; 

public interface ITimeConverter {
    public int ParseTime(string time);
    public string FormatTime(int time);
}