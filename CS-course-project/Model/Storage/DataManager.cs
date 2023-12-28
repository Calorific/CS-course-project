using System.Collections.Generic;
using System.Threading.Tasks;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Timetables;
using CS_course_project.Model.Validations;

namespace CS_course_project.Model.Storage; 

public class DataManager : IDataManager {

    #region Fields

    private readonly IValidationManager _validationManager;
    private readonly IRepository<string, List<string>, string> _groupsRepository;
    private readonly IRepository<string, List<string>, string> _classroomsRepository;
    private readonly IRepository<string, List<string>, string> _subjectsRepository;
    private readonly IRepository<ITeacher, List<ITeacher>, string> _teachersRepository;
    private readonly IRepository<ITimetable, Dictionary<string, ITimetable>, string> _timetablesRepository;
    private readonly IRepository<ISettings, ISettings, bool> _settingsRepository;
    private readonly IRepository<ISession, ISession?, bool> _sessionRepository;

    #endregion

    public DataManager(
        IValidationManager validationManager,
        IRepository<string, List<string>, string> groupsRepository,
        IRepository<string, List<string>, string> classroomsRepository,
        IRepository<string, List<string>, string> subjectsRepository,
        IRepository<ITeacher, List<ITeacher>, string> teachersRepository,
        IRepository<ITimetable, Dictionary<string, ITimetable>, string> timetablesRepository,
        IRepository<ISettings,ISettings,bool> settingsRepository,
        IRepository<ISession, ISession?, bool> sessionRepository
    ) {
        _validationManager = validationManager;
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

    public async Task<bool> AddTimetable(ITimetable newTimetable) {
        await _validationManager.ValidateNewTimetable(this, newTimetable);
        return await _timetablesRepository.Update(newTimetable);
    }

    #endregion
    

    #region Session

    public async Task<ISession?> GetSession() => await _sessionRepository.Read();
    
    public async Task<bool> UpdateSession(ISession newItem) => await _sessionRepository.Update(newItem);
    
    public async Task<bool> RemoveSession() => await _sessionRepository.Delete(true);

    #endregion


    #region Groups
    
    public async Task<List<string>> GetGroups() => await _groupsRepository.Read();
    
    public async Task<bool> UpdateGroups(string newGroup) {
        await _validationManager.ValidateNewGroup(this, newGroup);
        return await _groupsRepository.Update(newGroup);
    }
    
    public async Task<bool> RemoveGroup(string item) {
        await _timetablesRepository.Delete(item);
        return await _groupsRepository.Delete(item);
    }

    #endregion
    
    
    #region Teachers
    
    public async Task<List<ITeacher>> GetTeachers() => await _teachersRepository.Read();
    
    public async Task<bool> UpdateTeachers(ITeacher newTeacher) {
        await _validationManager.ValidateNewTeacher(this, newTeacher);
        return await _teachersRepository.Update(newTeacher);
    }
    
    public async Task<bool> RemoveTeacher(string id) {
        await _validationManager.ValidateRemovingTeacher(this, id);
        return await _teachersRepository.Delete(id);
    }

    #endregion
    
    
    #region Classrooms
    
    public async Task<List<string>> GetClassrooms() => await _classroomsRepository.Read();
    public async Task<bool> UpdateClassrooms(string newClassroom) {
        await _validationManager.ValidateNewClassroom(this, newClassroom);
        return await _classroomsRepository.Update(newClassroom);
    }
    public async Task<bool> RemoveClassroom(string classroom) {
        await _validationManager.ValidateRemovingClassroom(this, classroom);
        return await _classroomsRepository.Delete(classroom);
    }

    #endregion
    
    
    #region Subjects
    
    public async Task<List<string>> GetSubjects() => await _subjectsRepository.Read();
    public async Task<bool> UpdateSubjects(string newSubject) {
        await _validationManager.ValidateNewSubject(this, newSubject);
        return await _subjectsRepository.Update(newSubject);
    }
    public async Task<bool> RemoveSubject(string subject) {
        await _validationManager.ValidateRemovingSubject(this, subject);
        return await _subjectsRepository.Delete(subject);
    }

    #endregion
    

    #region Settings

    public async Task<ISettings> GetSettings() => await _settingsRepository.Read();
    public async Task UpdateSettings(ISettings settings) => await _settingsRepository.Update(settings);

    #endregion
    
}