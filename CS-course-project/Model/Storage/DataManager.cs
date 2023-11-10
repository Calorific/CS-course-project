
namespace CS_course_project.model.Storage; 

public static class DataManager {
    public static BaseRepository GroupRepository { get; } = new BaseRepository(RepositoryItems.Groups);
    public static BaseRepository TeachersRepository { get; } = new BaseRepository(RepositoryItems.Teachers);
    public static BaseRepository ClassroomsRepository { get; } = new BaseRepository(RepositoryItems.Classrooms);
    public static BaseRepository SubjectsRepository { get; } = new BaseRepository(RepositoryItems.Subjects);
    public static TimetableRepository TimetableRepository { get; } = new TimetableRepository();
    public static SettingsRepository SettingsRepository { get; } = new SettingsRepository();
}