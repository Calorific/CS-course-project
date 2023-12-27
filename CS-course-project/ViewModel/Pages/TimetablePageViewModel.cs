using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Input;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Services.TimeServices;
using CS_course_project.Model.Storage;
using CS_course_project.Model.Timetables;
using CS_course_project.ViewModel.Commands;
using CS_course_project.ViewModel.Common;

namespace CS_course_project.ViewModel.Pages;

public class LessonViewModel : BaseViewModel {
    public string Time { get; }
    public string Classroom { get; }
    public ITeacher Teacher { get; }
    public string Subject { get; }

    public LessonViewModel(string time, string classroom, ITeacher teacher, string subject) {
        Time = time;
        Classroom = classroom;
        Teacher = teacher;
        Subject = subject;
    }
}

public class DayViewModel {
    public string Name { get; }
    public List<LessonViewModel> Lessons { get; }

    public DayViewModel(string name) {
        Name = name;
        Lessons = new List<LessonViewModel>();
    }
}

public class TimetablePageViewModel : BaseViewModel {
    private readonly INavigator? _navigator;
    private readonly IDataManager? _dataManager;
    private readonly IAuthService? _authService;
    private readonly ITimeConverter? _timeConverter;
    
    private readonly string[] _names = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
    
    public ICommand LogOutCommand => Command.Create(LogOut);
    private async void LogOut(object? sender, EventArgs e) {
        if (_authService == null || _navigator == null) return;
        await _authService.LogOut();
        _navigator.Navigate(Routes.Login);
    }

    private string _title = string.Empty;
    public string Title {
        get => _title;
        private set {
            _title = value;
            Notify();
        }
    }

    private List<DayViewModel> _days = new();
    public List<DayViewModel> Days {
        get => _days;
        private set {
            _days = value;
            Notify();
        }
    }

    private void TransformTimetable(ITimetable timetable, IList<ITeacher> teachers, ISettings settings) {
        if (_timeConverter == null) return;
        var days = new List<DayViewModel>();
        var daysNumber = int.Parse(ConfigurationManager.AppSettings["NumberOfDays"]!);
        
        for (var i = 0; i < daysNumber; i++) {
            days.Add(new DayViewModel(_names[i % _names.Length]));
            
            if (i >= timetable.Days.Count) {
                continue;
            }

            var time = settings.StartTime;
            var lessons = timetable.Days[i].Lessons;
            for (var k = 0; k < lessons.Count; k++) {
                if (lessons[k] != null) {
                    var formattedTime = _timeConverter.FormatTime(time) + "-" +
                                        _timeConverter.FormatTime(time + settings.LessonDuration);
                    var teacher = teachers.FirstOrDefault(t => t.Id == lessons[k]!.Teacher);
                   
                    days[i].Lessons.Add(
                        new LessonViewModel(
                                formattedTime,
                                lessons[k]!.Classroom,
                                teacher ?? new Teacher("Не назначен", lessons[k]!.Teacher) as ITeacher,
                                lessons[k]!.Subject
                            )
                    );
                }

                time += settings.LessonDuration;
                time += settings.LongBreakLessons.Contains(k) ? settings.LongBreakDuration : settings.BreakDuration;
            }
        }

        Days = days;
    }
    
    private async void Init() {
        if (_dataManager == null) return;
        var session = await _dataManager.GetSession();
        var timetables = await _dataManager.GetTimetables();
        var teachers = await _dataManager.GetTeachers();
        var settings = await _dataManager.GetSettings();
        
        if (session == null) return;
        
        Title = "Расписание " + session.Data;
        TransformTimetable(timetables[session.Data], teachers, settings);
    }
    
    public TimetablePageViewModel(INavigator navigator, IDataManager dataManager, IAuthService authService, ITimeConverter timeConverter) {
        _navigator = navigator;
        _dataManager = dataManager;
        _authService = authService;
        _timeConverter = timeConverter;
        Init();
    }
    
    public TimetablePageViewModel() {}
}