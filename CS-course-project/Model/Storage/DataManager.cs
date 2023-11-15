
using System.Collections.Generic;
using System.Threading.Tasks;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Storage; 

public static class DataManager {

    #region Repositories

    private static BaseRepository GroupRepository { get; } = new(RepositoryItems.Groups);
    private static BaseRepository TeachersRepository { get; } = new(RepositoryItems.Teachers);
    private static BaseRepository ClassroomsRepository { get; } = new(RepositoryItems.Classrooms);
    private static BaseRepository SubjectsRepository { get; } = new(RepositoryItems.Subjects);
    
    private static TimetableRepository TimetableRepository { get; } = new();
    private static SettingsRepository SettingsRepository { get; } = new();

    #endregion
    


    #region Groups
    
    public static async Task<List<string>> LoadGroups() => await GroupRepository.GetData();
    public static async Task<bool> UpdateGroups(string newItem) => await GroupRepository.Update(newItem);
    public static async Task<bool> GroupsRemoveAt(int idx) => await GroupRepository.RemoveAt(idx);

    #endregion
    
    
    #region Teachers
    
    public static async Task<List<string>> LoadTeachers() => await TeachersRepository.GetData();
    public static async Task<bool> UpdateTeachers(string newItem) => await TeachersRepository.Update(newItem);
    public static async Task<bool> TeachersRemoveAt(int idx) => await TeachersRepository.RemoveAt(idx);

    #endregion
    
    
    #region Classrooms
    
    public static async Task<List<string>> LoadClassrooms() => await ClassroomsRepository.GetData();
    public static async Task<bool> UpdateClassrooms(string newItem) => await ClassroomsRepository.Update(newItem);
    public static async Task<bool> ClassroomsRemoveAt(int idx) => await ClassroomsRepository.RemoveAt(idx);

    #endregion
    
    
    #region Subjects
    
    public static async Task<List<string>> LoadSubjects() => await SubjectsRepository.GetData();
    public static async Task<bool> UpdateSubjects(string newItem) => await SubjectsRepository.Update(newItem);
    public static async Task<bool> SubjectsRemoveAt(int idx) => await SubjectsRepository.RemoveAt(idx);

    #endregion
    

    #region Settings

    public static async Task<Settings> LoadSettings() => await SettingsRepository.GetData();
    public static async Task UpdateSettings(Settings settings) => await SettingsRepository.Update(settings);

    #endregion
    

}