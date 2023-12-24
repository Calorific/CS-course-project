using System.Collections.Generic;
using System.Threading.Tasks;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Storage; 

public class DataManager : IDataManager {

    #region Repositories

    private readonly IRepository<string, List<string>, string> _groupsRepository;
    private readonly IRepository<string, List<string>, string> _classroomsRepository;
    private readonly IRepository<string, List<string>, string> _subjectsRepository;
    private readonly IRepository<ITeacher, List<ITeacher>, string> _teachersRepository;
    private readonly IRepository<ITimetable, Dictionary<string, ITimetable>, string> _timetablesRepository;
    private readonly IRepository<ISettings, ISettings, bool> _settingsRepository;
    private readonly IRepository<ISession, ISession?, bool> _sessionRepository;

    #endregion

    public DataManager(
        IRepository<string, List<string>, string> groupsRepository,
        IRepository<string, List<string>, string> classroomsRepository,
        IRepository<string, List<string>, string> subjectsRepository,
        IRepository<ITeacher, List<ITeacher>, string> teachersRepository,
        IRepository<ITimetable, Dictionary<string, ITimetable>, string> timetablesRepository,
        IRepository<ISettings,ISettings,bool> settingsRepository,
        IRepository<ISession, ISession?, bool> sessionRepository
    ) {
        _groupsRepository = groupsRepository;
        _classroomsRepository = classroomsRepository;
        _subjectsRepository = subjectsRepository;
        _teachersRepository = teachersRepository;
        _timetablesRepository = timetablesRepository;
        _settingsRepository = settingsRepository;
        _sessionRepository = sessionRepository;
    }
    
    
    #region Timetable

    public async Task<Dictionary<string, ITimetable>> GetTimetables() => await _timetablesRepository.Read();
    public async Task<bool> AddTimetable(ITimetable newItem) => await _timetablesRepository.Update(newItem);
    private async Task RemoveTimeTable(string key) => await _timetablesRepository.Delete(key);

    #endregion
    

    #region Session

    public async Task<ISession?> GetSession() => await _sessionRepository.Read();
    public async Task<bool> UpdateSession(ISession newItem) => await _sessionRepository.Update(newItem);
    public async Task<bool> RemoveSession() => await _sessionRepository.Delete(true);

    #endregion


    #region Groups
    
    public async Task<List<string>> GetGroups() => await _groupsRepository.Read();
    public async Task<bool> UpdateGroups(string newItem) => await _groupsRepository.Update(newItem);
    public async Task<bool> RemoveGroup(string item) {
        await RemoveTimeTable(item);
        return await _groupsRepository.Delete(item);
    }

    #endregion
    
    
    #region Teachers
    
    public async Task<List<ITeacher>> GetTeachers() => await _teachersRepository.Read();
    public async Task<bool> UpdateTeachers(Teacher newItem) => await _teachersRepository.Update(newItem);
    public async Task<bool> RemoveTeacher(string id) => await _teachersRepository.Delete(id);

    #endregion
    
    
    #region Classrooms
    
    public async Task<List<string>> GetClassrooms() => await _classroomsRepository.Read();
    public async Task<bool> UpdateClassrooms(string newItem) => await _classroomsRepository.Update(newItem);
    public async Task<bool> RemoveClassroom(string item) => await _classroomsRepository.Delete(item);

    #endregion
    
    
    #region Subjects
    
    public async Task<List<string>> GetSubjects() => await _subjectsRepository.Read();
    public async Task<bool> UpdateSubjects(string newItem) => await _subjectsRepository.Update(newItem);
    public async Task<bool> RemoveSubject(string item) => await _subjectsRepository.Delete(item);

    #endregion
    

    #region Settings

    public async Task<ISettings> GetSettings() => await _settingsRepository.Read();
    public async Task UpdateSettings(ISettings settings) => await _settingsRepository.Update(settings);

    #endregion
    
}