using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Timetables;

namespace CS_course_project.Model.Storage; 

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
    public async Task<bool> AddTimetable(ITimetable newTimetable) {
        var timetables = await GetTimetables();
        var settings = await GetSettings();
        foreach (var group in timetables.Keys) {
            if (group == newTimetable.Group) continue;
            
            var currentTimetable = timetables[group];
            for (var i = 0; i < int.Parse(ConfigurationManager.AppSettings["NumberOfDays"]!); i++) {
                if (i >= currentTimetable.Days.Count) break;
                
                for (var k = 0; k < settings.LessonsNumber; k++) {
                    if (k >= currentTimetable.Days[i].Lessons.Count) break;
                    
                    var currentLesson = currentTimetable.Days[i].Lessons[k];
                    var newLesson = newTimetable.Days[i].Lessons[k];

                    if (currentLesson == null || newLesson == null) continue;
                    
                    if (currentLesson.Classroom == newLesson.Classroom) {
                        throw new ArgumentException(
                            $"Аудитория {newLesson.Classroom} занята группой {group}");
                    }
                        
                    if (currentLesson.Teacher == newLesson.Teacher) {
                        throw new ArgumentException(
                            $"Преподаватель {newLesson.Teacher} в этот момент у группы {group}");
                    }
                }
                
            }
        }
        return await _timetablesRepository.Update(newTimetable);
    }
    private async Task RemoveTimeTable(string key) => await _timetablesRepository.Delete(key);

    #endregion
    

    #region Session

    public async Task<ISession?> GetSession() => await _sessionRepository.Read();
    public async Task<bool> UpdateSession(ISession newItem) => await _sessionRepository.Update(newItem);
    public async Task<bool> RemoveSession() => await _sessionRepository.Delete(true);

    #endregion


    #region Groups
    
    public async Task<List<string>> GetGroups() => await _groupsRepository.Read();
    public async Task<bool> UpdateGroups(string newGroup) {
        var groups = await GetGroups();
        if (groups.Contains(newGroup)) {
            throw new ArgumentException("Такая группа уже существует");
        }
        return await _groupsRepository.Update(newGroup);
    }
    public async Task<bool> RemoveGroup(string item) {
        await RemoveTimeTable(item);
        return await _groupsRepository.Delete(item);
    }

    #endregion
    
    
    #region Teachers
    
    public async Task<List<ITeacher>> GetTeachers() => await _teachersRepository.Read();
    public async Task<bool> UpdateTeachers(Teacher newTeacher) {
        var teachers = await GetTeachers();
        if (teachers.Any(t => t.Id == newTeacher.Id))
            throw new ArgumentException("Учитель уже существует");
        return await _teachersRepository.Update(newTeacher);
    }
    public async Task<bool> RemoveTeacher(string id) {
        var group = await GetUsingGroup("Teacher", id);
        if (group != null)
            throw new ArgumentException("Преподаватель в расписании " + group);
        return await _teachersRepository.Delete(id);
    }

    #endregion
    
    
    #region Classrooms
    
    public async Task<List<string>> GetClassrooms() => await _classroomsRepository.Read();
    public async Task<bool> UpdateClassrooms(string newClassroom) {
        var classrooms = await GetClassrooms();
        if (classrooms.Contains(newClassroom))
            throw new ArgumentException("Аудитория уже существует");
        return await _classroomsRepository.Update(newClassroom);
    }
    public async Task<bool> RemoveClassroom(string id) {
        var group = await GetUsingGroup("Classroom", id);
        if (group != null)
            throw new ArgumentException("Аудитория в расписании " + group);
        return await _classroomsRepository.Delete(id);
    }

    #endregion
    
    
    #region Subjects
    
    public async Task<List<string>> GetSubjects() => await _subjectsRepository.Read();
    public async Task<bool> UpdateSubjects(string newSubject) {
        var subjects = await GetSubjects();
        if (subjects.Contains(newSubject))
            throw new ArgumentException("Предмет уже существует");
        return await _subjectsRepository.Update(newSubject);
    }
    public async Task<bool> RemoveSubject(string id) {
        var group = await GetUsingGroup("Subject", id);
        if (group != null)
            throw new ArgumentException("Предмет в расписании " + group);
        return await _subjectsRepository.Delete(id);
    }

    #endregion
    

    #region Settings

    public async Task<ISettings> GetSettings() => await _settingsRepository.Read();
    public async Task UpdateSettings(ISettings settings) => await _settingsRepository.Update(settings);

    #endregion
    
    
    private async Task<string?> GetUsingGroup(string field, string value) {
        var timetables = await GetTimetables();
        
        foreach (var group in timetables.Keys) {
            foreach (var day in timetables[group].Days) {
                foreach (var lesson in day.Lessons) {
                    if (lesson == null) continue;
                    switch (field) {
                        case "Teacher" when lesson.Teacher == value:
                        case "Classroom" when lesson.Classroom == value:
                        case "Subject" when lesson.Subject == value:
                            return group;
                    }
                }
            }
        }

        return null;
    }
}