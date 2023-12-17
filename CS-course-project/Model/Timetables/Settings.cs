using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CS_course_project.Model.Timetables; 

public class Settings : ISettings {
    public string HashedAdminPassword { get; } = BCrypt.Net.BCrypt.HashPassword("123");

    private readonly int _lessonDuration = 40;
    public int LessonDuration {
        get => _lessonDuration;
        private init {
            if (value > 24 * 60)
                throw new ArgumentException("Некорректное значение длины урока");
            _lessonDuration = value;
        }}
    
    private readonly int _breakDuration = 10;
    public int BreakDuration {
        get => _breakDuration;
        private init {
            if (value > 24 * 60)
                throw new ArgumentException("Некорректное значение перемены");
            _breakDuration = value;
        }}
    
    private readonly int _longBreakDuration = 10;
    public int LongBreakDuration {
        get => _longBreakDuration;
        private init {
            if (value > 24 * 60)
                throw new ArgumentException("Некорректное значение большой перемены");
            _longBreakDuration = value;
        }}
    
    private readonly int _startTime = 480;
    public int StartTime {
        get => _startTime;
        private init {
            if (value > 24 * 60)
                throw new ArgumentException("Некорректное значение начала занятий");
            _startTime = value;
        }
    }
    
    private readonly int _lessonsNumber = 14;
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
        HashedAdminPassword = hashedAdminPassword;
        LessonDuration = lessonDuration;
        BreakDuration = breakDuration;
        LongBreakDuration = longBreakDuration;
        StartTime = startTime;
        LessonsNumber = lessonsNumber;
        LongBreakLessons = longBreakLessons;
    }
}