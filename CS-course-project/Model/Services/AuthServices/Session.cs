using System;

namespace CS_course_project.Model.Services.AuthServices; 

public class Session : ISession {
    public bool IsAdmin { get; }
    public string Data { get; }

    public Session(bool isAdmin, string data) {
        if (string.IsNullOrEmpty(data))
            throw new ArgumentException("Данные сессии не могут быть пустыми");
        IsAdmin = isAdmin;
        Data = data;
    }
}