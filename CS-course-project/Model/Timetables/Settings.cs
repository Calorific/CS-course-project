using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json.Serialization;

namespace CS_course_project.Model.Timetables; 

public class Settings : ISettings {
    public string HashedAdminPassword { get; } = BCrypt.Net.BCrypt.HashPassword(ConfigurationManager.AppSettings["DefaultHashedAdminPassword"] ?? "123");

    private readonly int _lessonDuration = int.Parse(ConfigurationManager.AppSettings["DefaultLessonDuration"] ?? "40");
    public int LessonDuration {
        get => _lessonDuration;
        private init {
            _lessonDuration = value switch {
                > 24 * 60 => throw new ArgumentException("Некорректное значение длины урока"),
                <= 0 => throw new ArgumentException("Длина урока должна быть больше 0"),
                _ => value
            };
        }}
    
    private readonly int _breakDuration = int.Parse(ConfigurationManager.AppSettings["DefaultBreakDuration"] ?? "10");
    public int BreakDuration {
        get => _breakDuration;
        private init {
            _breakDuration = value switch {
                > 24 * 60 => throw new ArgumentException("Некорректное значение перемены"),
                < 0 => throw new ArgumentException("Перемена не может быть отрицательной"),
                _ => value
            };
        }}
    
    private readonly int _longBreakDuration = int.Parse(ConfigurationManager.AppSettings["DefaultLongBreakDuration"] ?? "15");
    public int LongBreakDuration {
        get => _longBreakDuration;
        private init {
            _longBreakDuration = value switch {
                > 24 * 60 => throw new ArgumentException("Некорректное значение большой перемены"),
                < 0 => throw new ArgumentException("Большая перемена не может быть отрицательной"),
                _ => value
            };
        }}
    
    private readonly int _startTime = int.Parse(ConfigurationManager.AppSettings["DefaultStartTime"] ?? "480");
    public int StartTime {
        get => _startTime;
        private init {
            _startTime = value switch {
                > 24 * 60 => throw new ArgumentException("Некорректное значение начала занятий"),
                < 0 => throw new ArgumentException("Время начала не может быть отрицательным"),
                _ => value
            };
        }
    }
    
    private readonly int _lessonsNumber = int.Parse(ConfigurationManager.AppSettings["DefaultLessonsNumber"] ?? "4");
    public int LessonsNumber {
        get => _lessonsNumber;
        private init {
            if (value is < 1 or > 99) 
                throw new ArgumentException("Некорректное значение количества уроков");
            _lessonsNumber = value;
        }
    }

    public List<int> LongBreakLessons { get; } = new();

    public Settings() {}

    [JsonConstructor]
    public Settings(int lessonDuration, int breakDuration, int longBreakDuration, int startTime, string hashedAdminPassword, int lessonsNumber, List<int> longBreakLessons) {
        if (string.IsNullOrEmpty(hashedAdminPassword))
            throw new ArgumentException("Пароль не должен быть пустым");
        
        if (longBreakLessons.Any(lesson => !(lesson < lessonsNumber - 1)))
            throw new ArgumentException("Некорректный список больших перемен");
        
        var totalTime = startTime + lessonsNumber * lessonDuration + longBreakDuration * longBreakLessons.Count
                        + breakDuration * (lessonsNumber - longBreakLessons.Count - 1);
        if (totalTime > 24 * 60)
            throw new ArgumentException("Общее время не должно превышать 24 часа");
        
        HashedAdminPassword = hashedAdminPassword;
        LessonDuration = lessonDuration;
        BreakDuration = breakDuration;
        LongBreakDuration = longBreakDuration;
        StartTime = startTime;
        LessonsNumber = lessonsNumber;
        LongBreakLessons = longBreakLessons;
    }
}