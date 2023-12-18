using System.Collections.Generic;
using System.Threading.Tasks;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Storage; 

public static class DataManager {

    #region Repositories

    private static BaseRepository GroupRepository { get; } = new(RepositoryItems.Groups);
    private static BaseRepository ClassroomsRepository { get; } = new(RepositoryItems.Classrooms);
    private static BaseRepository SubjectsRepository { get; } = new(RepositoryItems.Subjects);
    
    private static TeacherRepository TeachersRepository { get; } = new();
    
    private static TimetableRepository TimetableRepository { get; } = new();
    
    private static SettingsRepository SettingsRepository { get; } = new();

    private static SessionRepository SessionRepository { get; } = new();

    #endregion

    #region Timetable

    public static async Task<Dictionary<string, ITimetable>> LoadTimetables() => await TimetableRepository.GetData();
    public static async Task<bool> AddTimetable(ITimetable newItem) => await TimetableRepository.Update(newItem);
    private static async Task RemoveTimeTable(string key) => await TimetableRepository.RemoveAt(key);

    #endregion
    

    #region Session

    public static async Task<ISession?> LoadSession() => await SessionRepository.GetData();
    public static async Task<bool> UpdateSession(ISession newItem) => await SessionRepository.Update(newItem);
    public static async Task<bool> RemoveSession() => await SessionRepository.RemoveAt(true);

    #endregion


    #region Groups
    
    public static async Task<List<string>> LoadGroups() => await GroupRepository.GetData();
    public static async Task<bool> UpdateGroups(string newItem) => await GroupRepository.Update(newItem);
    public static async Task<bool> RemoveGroup(string item) {
        await RemoveTimeTable(item);
        return await GroupRepository.RemoveAt(item);
    }

    #endregion
    
    
    #region Teachers
    
    public static async Task<List<ITeacher>> LoadTeachers() => await TeachersRepository.GetData();
    public static async Task<bool> UpdateTeachers(Teacher newItem) => await TeachersRepository.Update(newItem);
    public static async Task<bool> RemoveTeacher(string id) => await TeachersRepository.RemoveAt(id);

    #endregion
    
    
    #region Classrooms
    
    public static async Task<List<string>> LoadClassrooms() => await ClassroomsRepository.GetData();
    public static async Task<bool> UpdateClassrooms(string newItem) => await ClassroomsRepository.Update(newItem);
    public static async Task<bool> RemoveClassroom(string item) => await ClassroomsRepository.RemoveAt(item);

    #endregion
    
    
    #region Subjects
    
    public static async Task<List<string>> LoadSubjects() => await SubjectsRepository.GetData();
    public static async Task<bool> UpdateSubjects(string newItem) => await SubjectsRepository.Update(newItem);
    public static async Task<bool> RemoveSubject(string item) => await SubjectsRepository.RemoveAt(item);

    #endregion
    

    #region Settings

    public static async Task<ISettings> LoadSettings() => await SettingsRepository.GetData();
    public static async Task UpdateSettings(ISettings settings) => await SettingsRepository.Update(settings);

    #endregion
    

}