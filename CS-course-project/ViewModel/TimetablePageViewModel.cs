﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.model.Storage;
using CS_course_project.Model.Timetables;
using CS_course_project.Navigation;

namespace CS_course_project.ViewModel;

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
    private readonly string[] _names = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
    
    public static ICommand LogOutCommand => Command.Create(LogOut);
    private static async void LogOut(object? sender, EventArgs e) {
        await AuthService.LogOut();
        Navigator.Navigate.Execute("Login", null);
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
        var days = new List<DayViewModel>();
        var daysNumber = int.Parse(ConfigurationManager.AppSettings["NumberOfDays"]!);
        
        for (var i = 0; i < daysNumber; i++) {
            if (i > timetable.Days.Count)
                break;

            var time = settings.StartTime;
            days.Add(new DayViewModel(_names[i % _names.Length]));
            var lessons = timetable.Days[i].Lessons;
            for (var k = 0; k < lessons.Count; k++) {
                if (lessons[k] != null) {
                    var formattedTime = TimeConverter.FormatTime(time) + "-" +
                                        TimeConverter.FormatTime(time + settings.LessonDuration);
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
        var session = await DataManager.LoadSession();
        var timetables = await DataManager.LoadTimetables();
        var teachers = await DataManager.LoadTeachers();
        var settings = await DataManager.LoadSettings();
        
        if (session == null) return;
        
        Title = "Расписание " + session.Data;
        TransformTimetable(timetables[session.Data], teachers, settings);
    }
    
    public TimetablePageViewModel() {
        Init();
    }
}