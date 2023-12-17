using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.model.Storage;
using CS_course_project.model.Timetables;
using CS_course_project.Model.Timetables;
using CS_course_project.Navigation;

namespace CS_course_project.ViewModel.UserControls.TimetableForm;

public class TimetableFormViewModel : NotifyErrorsViewModel {
    private readonly string[] _names = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
    
    public ICommand SubmitCommand => Command.Create(Submit);
    private async void Submit(object? sender, EventArgs e) {
        if (_currentTimetable.Days.Any(day => day.Lessons.Any(lesson => lesson.HasErrors)))
            return;
        var days = new List<IDay>();
        foreach (var day in _currentTimetable.Days) {
            var lessons = new List<ILesson?>();
            foreach (var lesson in day.Lessons) {
                if (string.IsNullOrEmpty(lesson.Classroom) && string.IsNullOrEmpty(lesson.Teacher) && string.IsNullOrEmpty(lesson.Subject))
                    lessons.Add(null);
                else
                    lessons.Add(new Lesson(lesson.Subject, lesson.Classroom, lesson.Teacher));
            }
            days.Add(new Day(lessons));
        }
        await DataManager.AddTimetable(new Timetable(_currentGroup, days));
    }


    private Dictionary<string, ITimetable>? _timetables;
    private TimetableViewModel _currentTimetable = new ("", new List<DayViewModel>());
    private bool _isFirstRender = true;
    
    public TimetableViewModel CurrentTimetable {
        get => _currentTimetable;
        private set {
            if (_currentTimetable == value) return;
            _currentTimetable = value;
            Notify();
        }
    }

    private List<string>? _groups;
    public List<string>? Groups {
        get => _groups;
        private set {
            _groups = value;
            Notify();
        }
    }

    private List<ITeacher>? _teachers;
    public List<ITeacher>? Teachers {
        get => _teachers;
        set {
            if (value == _teachers) return;
            _teachers = value;
            Notify();
        }
    }
    
    private List<string>? _classrooms;
    public List<string>? Classrooms {
        get => _classrooms;
        set {
            if (value == _classrooms) return;
            _classrooms = value;
            Notify();
        }
    }

    private List<string>? _subjects;
    public List<string>? Subjects {
        get => _subjects;
        set {
            if (value == _subjects) return;
            _subjects = value;
            Notify();
        }
    }
    private ISettings? _settings;
    
    private string _currentGroup = "";
    public string CurrentGroup {
        get => _currentGroup;
        set {
            _currentGroup = value;
            ClearErrors(nameof(CurrentGroup));
            if (Groups != null && !Groups.Contains(_currentGroup))
                AddError(nameof(CurrentGroup), "Нужно выбрать существующую группу");
            else if (_timetables != null) {
                _timetables.TryGetValue(value, out var timetable);
                UpdateTimetable(timetable, value);
            }
            Notify();
        }
    }

    private string? _validateSubject(int dayIdx, int lessonIdx) {
        var lesson = _currentTimetable.Days[dayIdx].Lessons[lessonIdx];
        if (_subjects != null && !_subjects.Contains(lesson.Subject)) {
            lesson.Subject = string.Empty;
            return "Предмета не существует";
        }
        return null;
    }
    private string? _validateClassroom(int dayIdx, int lessonIdx) {
        var lesson = _currentTimetable.Days[dayIdx].Lessons[lessonIdx];
        if (_classrooms != null && !_classrooms.Contains(lesson.Classroom)) {
            lesson.Classroom = string.Empty;
            return "Аудитории не существует";
        }

        if (_timetables == null) return null;
        foreach (var group in _timetables.Keys) {
            if (group == _currentGroup) continue;
            if (dayIdx < 0 || dayIdx >= _timetables[group].Days.Count || lessonIdx < 0 ||
                lessonIdx >= _timetables[group].Days[dayIdx].Lessons.Count) continue;
            if (_timetables[group].Days[dayIdx].Lessons[lessonIdx]?.Classroom == lesson.Classroom)
                return "В аудитории уже группа " + group;
        }
        return null;
    }
    private string? _validateTeacher(int dayIdx, int lessonIdx) {
        var lesson = _currentTimetable.Days[dayIdx].Lessons[lessonIdx];
        if (_teachers != null && _teachers.Find(t => t.Id == lesson.Teacher) == null) {
            lesson.Teacher = string.Empty;
            return "Преподаватель не существует";
        }
        if (_timetables == null) return null;
        foreach (var group in _timetables.Keys) {
            if (group == _currentGroup) continue;
            if (dayIdx < 0 || dayIdx >= _timetables[group].Days.Count || lessonIdx < 0 ||
                lessonIdx >= _timetables[group].Days[dayIdx].Lessons.Count) continue;
            if (_timetables[group].Days[dayIdx].Lessons[lessonIdx]?.Teacher == lesson.Teacher)
                return "Преподаватель уже у группы " + group;
        }
        return null;
    }
    
    private void UpdateTimetable(ITimetable? timetable, string group) {
        if (_isFirstRender) {
            IList<DayViewModel> days = Enumerable
                .Range(0, int.Parse(ConfigurationManager.AppSettings["NumberOfDays"]!))
                .Select(dayIdx => new DayViewModel(Enumerable.Range(0, _settings!.LessonsNumber)
                    .Select(lessonIdx => new LessonViewModel(dayIdx, lessonIdx, _validateSubject, _validateClassroom, _validateTeacher))
                    .ToList(), _names[dayIdx % _names.Length]))
                .ToList();
            CurrentTimetable = new TimetableViewModel(group, days);
            _isFirstRender = false;
        }
        
        if (timetable == null) {
            foreach (var day in CurrentTimetable.Days) {
                foreach (var lesson in day.Lessons) {
                    lesson.Classroom = lesson.Teacher = lesson.Subject = string.Empty;
                }
            }
            return;
        }
        for (var i = 0; i < int.Parse(ConfigurationManager.AppSettings["NumberOfDays"]!); i++) {
            for (var k = 0; k < _settings!.LessonsNumber; k++) {
                if (i < timetable.Days?.Count && k < timetable.Days[i].Lessons.Count) {
                    CurrentTimetable.Days[i].Lessons[k].Classroom = timetable.Days[i].Lessons[k]?.Classroom ?? string.Empty;
                    CurrentTimetable.Days[i].Lessons[k].Teacher = timetable.Days[i].Lessons[k]?.Teacher ?? string.Empty;
                    CurrentTimetable.Days[i].Lessons[k].Subject = timetable.Days[i].Lessons[k]?.Subject ?? string.Empty;
                }
                else {
                    CurrentTimetable.Days[i].Lessons[k].Classroom = CurrentTimetable.Days[i].Lessons[k].Teacher
                        = CurrentTimetable.Days[i].Lessons[k].Subject = string.Empty;
                }
            }
        }
    }
    
    private async void CheckRedirect() {
        Groups = new List<string>((await DataManager.LoadGroups()).OrderBy(s => s.ToLower()));
        _teachers = new List<ITeacher>((await DataManager.LoadTeachers()).OrderBy(t => t.Name.ToLower()));
        _classrooms = new List<string>((await DataManager.LoadClassrooms()).OrderBy(s => s.ToLower()));
        _subjects = new List<string>((await DataManager.LoadSubjects()).OrderBy(s => s.ToLower()));
        _timetables = await DataManager.LoadTimetables();
        _settings = await DataManager.LoadSettings();
        
        if (Groups.Count == 0 || _teachers.Count == 0 || _classrooms.Count == 0 || _subjects.Count == 0) {
            Navigator.Navigate.Execute("Settings", null);
            return;
        }
        
        CurrentGroup = Groups[0];
    }

    public TimetableFormViewModel() {
        CheckRedirect();
    }
}