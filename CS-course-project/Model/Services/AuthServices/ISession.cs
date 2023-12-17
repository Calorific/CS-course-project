namespace CS_course_project.Model.Services.AuthServices; 

public interface ISession {
    public bool IsAdmin { get; }
    public string Data { get; }
}