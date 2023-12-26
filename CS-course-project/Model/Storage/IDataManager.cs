using System.Collections.Generic;
using System.Threading.Tasks;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Timetables;

namespace CS_course_project.Model.Storage; 

public interface IDataManager {
    #region Timetable

    public Task<Dictionary<string, ITimetable>> GetTimetables();
    public Task<bool> AddTimetable(ITimetable newTimetable);

    #endregion
    

    #region Session

    public Task<ISession?> GetSession();
    public Task<bool> UpdateSession(ISession newItem);
    public Task<bool> RemoveSession();

    #endregion


    #region Groups
    
    public Task<List<string>> GetGroups();
    public Task<bool> UpdateGroups(string newItem);
    public Task<bool> RemoveGroup(string item);

    #endregion
    
    
    #region Teachers
    
    public Task<List<ITeacher>> GetTeachers();
    public Task<bool> UpdateTeachers(Teacher newItem);
    public Task<bool> RemoveTeacher(string id);

    #endregion
    
    
    #region Classrooms
    
    public Task<List<string>> GetClassrooms();
    public Task<bool> UpdateClassrooms(string newClassroom);
    public Task<bool> RemoveClassroom(string item);

    #endregion
    
    
    #region Subjects
    
    public Task<List<string>> GetSubjects();
    public Task<bool> UpdateSubjects(string newSubject);
    public Task<bool> RemoveSubject(string item);

    #endregion
    

    #region Settings

    public Task<ISettings> GetSettings();
    public Task UpdateSettings(ISettings settings);
    
    #endregion Settings
}