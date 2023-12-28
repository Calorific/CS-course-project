using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CS_course_project.Model.Storage;
using CS_course_project.Model.Timetables;

namespace CS_course_project.Model.Validations; 

public class ValidationManager : IValidationManager {
    public async Task ValidateNewTimetable(IDataManager dataManager, ITimetable newTimetable) {
        var timetables = await dataManager.GetTimetables();
        var classrooms = await dataManager.GetClassrooms();
        var subjects = await dataManager.GetSubjects();
        var teachers = await dataManager.GetTeachers();
        var settings = await dataManager.GetSettings();

        await Task.Run(() => {
            foreach (var day in newTimetable.Days) {
                foreach (var lesson in day.Lessons) {
                    if (lesson == null)
                        continue;
                    
                    if (!classrooms.Contains(lesson.Classroom))
                        throw new ArgumentException("Аудитория не существует");
                    if (!subjects.Contains(lesson.Subject))
                        throw new ArgumentException("Предмет не существует");
                    if (teachers.FirstOrDefault(t => t.Id == lesson.Teacher) == null)
                        throw new ArgumentException("Преподаватель не существует");
                }
            }
            
            foreach (var group in timetables.Keys) {
                if (group == newTimetable.Group) continue;

                var currentTimetable = timetables[group];
                for (var i = 0; i < int.Parse(ConfigurationManager.AppSettings["NumberOfDays"] ?? "6"); i++) {
                    if (i >= currentTimetable.Days.Count) break;

                    for (var k = 0; k < settings.LessonsNumber; k++) {
                        if (k >= currentTimetable.Days[i].Lessons.Count)
                            break;

                        var currentLesson = currentTimetable.Days[i].Lessons[k];
                        var newLesson = newTimetable.Days[i].Lessons[k];

                        if (newLesson == null || currentLesson == null)
                            continue;

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
        });
    }

    public async Task ValidateNewGroup(IDataManager dataManager, string newGroup) {
        if (string.IsNullOrEmpty(newGroup)) {
            throw new ArgumentException("Группа не может быть пустой");
        }
        
        var groups = await dataManager.GetGroups();
        
        if (groups.Contains(newGroup)) {
            throw new ArgumentException("Такая группа уже существует");
        }
    }
    
    public async Task ValidateNewTeacher(IDataManager dataManager, ITeacher newTeacher) {
        var teachers = await dataManager.GetTeachers();
        
        if (teachers.FirstOrDefault(t => t.Id == newTeacher.Id) != null) {
            throw new ArgumentException("Преподаватель уже существует");
        }
    }

    public async Task ValidateRemovingTeacher(IDataManager dataManager, string id) {
        var group = await GetUsingGroup(dataManager, "Teacher", id);
        
        if (group != null) {
            throw new ArgumentException("Преподаватель в расписании " + group);
        }
    }
    
    public async Task ValidateNewClassroom(IDataManager dataManager, string newClassroom) {
        if (string.IsNullOrEmpty(newClassroom)) {
            throw new ArgumentException("Аудитория не может быть пустой");
        }
        
        var classrooms = await dataManager.GetClassrooms();
        if (classrooms.Contains(newClassroom)) {
            throw new ArgumentException("Аудитория уже существует");
        }
    }
    
    public async Task ValidateRemovingClassroom(IDataManager dataManager, string item) {
        var group = await GetUsingGroup(dataManager, "Classroom", item);
        
        if (group != null) {
            throw new ArgumentException("Аудитория в расписании " + group);
        }
    }
    
    public async Task ValidateNewSubject(IDataManager dataManager, string newSubject) {
        if (string.IsNullOrEmpty(newSubject)) {
            throw new ArgumentException("Предмет не может быть пустым");
        }
        
        var subjects = await dataManager.GetSubjects();
        if (subjects.Contains(newSubject)) {
            throw new ArgumentException("Предмет уже существует");
        }
    }
    
    public async Task ValidateRemovingSubject(IDataManager dataManager, string item) {
        var group = await GetUsingGroup(dataManager, "Subject", item);
        
        if (group != null) {
            throw new ArgumentException("Аудитория в расписании " + group);
        }
    }
    
    private static async Task<string?> GetUsingGroup(IDataManager dataManager, string field, string value) {
        var timetables = await dataManager.GetTimetables();

        return await Task.Run(() => {
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
        });
    }
}