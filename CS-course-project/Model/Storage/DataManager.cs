
using System.Collections.Generic;
using System.Threading.Tasks;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Storage; 

public static class DataManager {
    private static BaseRepository GroupRepository { get; } = new BaseRepository(RepositoryItems.Groups);
    private static BaseRepository TeachersRepository { get; } = new BaseRepository(RepositoryItems.Teachers);
    private static BaseRepository ClassroomsRepository { get; } = new BaseRepository(RepositoryItems.Classrooms);
    private static BaseRepository SubjectsRepository { get; } = new BaseRepository(RepositoryItems.Subjects);
    
    private static TimetableRepository TimetableRepository { get; } = new TimetableRepository();
    private static SettingsRepository SettingsRepository { get; } = new SettingsRepository();
    
    public static async Task<List<string>> LoadGroups() => await GroupRepository.GetData();
    
    public static async Task<List<string>> LoadTeachers() => await TeachersRepository.GetData();
    
    public static async Task<List<string>> LoadClassrooms() => await ClassroomsRepository.GetData();
    
    public static async Task<List<string>> LoadSubjects() => await SubjectsRepository.GetData();
    
    public static async Task<Settings> LoadSettings() => await SettingsRepository.GetData();
    
}