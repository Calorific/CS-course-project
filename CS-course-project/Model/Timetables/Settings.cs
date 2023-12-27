using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json.Serialization;

namespace CS_course_project.Model.Timetables; 

public class Settings : ISettings {
    public string HashedAdminPassword { get; } = BCrypt.Net.BCrypt.HashPassword(ConfigurationManager.AppSettings["DefaultHashedAdminPassword"]!);

    private readonly int _lessonDuration = int.Parse(ConfigurationManager.AppSettings["DefaultLessonDuration"]!);
    public int LessonDuration {
        get => _lessonDuration;
        private init {
            if (value > 24 * 60)
                throw new ArgumentException("Некорректное значение длины урока");
            _lessonDuration = value;
        }}
    
    private readonly int _breakDuration = int.Parse(ConfigurationManager.AppSettings["DefaultBreakDuration"]!);
    public int BreakDuration {
        get => _breakDuration;
        private init {
            if (value > 24 * 60)
                throw new ArgumentException("Некорректное значение перемены");
            _breakDuration = value;
        }}
    
    private readonly int _longBreakDuration = int.Parse(ConfigurationManager.AppSettings["DefaultLongBreakDuration"]!);
    public int LongBreakDuration {
        get => _longBreakDuration;
        private init {
            if (value > 24 * 60)
                throw new ArgumentException("Некорректное значение большой перемены");
            _longBreakDuration = value;
        }}
    
    private readonly int _startTime = int.Parse(ConfigurationManager.AppSettings["DefaultStartTime"]!);
    public int StartTime {
        get => _startTime;
        private init {
            if (value > 24 * 60)
                throw new ArgumentException("Некорректное значение начала занятий");
            _startTime = value;
        }
    }
    
    private readonly int _lessonsNumber = int.Parse(ConfigurationManager.AppSettings["DefaultLessonsNumber"]!);
    public int LessonsNumber {
        get => _lessonsNumber;
        private init {
            if (value is < 0 or > 100) 
                throw new ArgumentException("Некорректное значение количества уроков");
            _lessonsNumber = value;
        }
    }

    public List<int> LongBreakLessons { get; } = new();

    public Settings() {}

    [JsonConstructor]
    public Settings(int lessonDuration, int breakDuration, int longBreakDuration, int startTime, string hashedAdminPassword, int lessonsNumber, List<int> longBreakLessons) {
        if (lessonDuration <= 0)
            throw new ArgumentException("Длина урока должна быть больше 0");
        if (breakDuration < 0)
            throw new ArgumentException("Перемена не может быть отрицательной");
        if (longBreakDuration < 0)
            throw new ArgumentException("Большая перемена не может быть отрицательной");
        if (startTime < 0)
            throw new ArgumentException("Время начала не может быть отрицательным");
        if (lessonsNumber < 0)
            throw new ArgumentException("Количество уроков должно быть больше 0");
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