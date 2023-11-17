using System;
using System.Text.Json.Serialization;

namespace CS_course_project.Model.Timetables; 

public class Settings {
    public string AdminPassword { get; private init; } = "123";

    private readonly int _lessonDuration = 40;
    public int LessonDuration {
        get => _lessonDuration;
        private init {
            if (value > 24 * 60)
                throw new Exception("Некорректное значение");
            _lessonDuration = value;
        }}
    
    private readonly int _breakDuration = 10;
    public int BreakDuration {
        get => _breakDuration;
        private init {
            if (value > 24 * 60)
                throw new Exception("Некорректное значение");
            _breakDuration = value;
        }}
    
    private readonly int _longBreakDuration = 10;
    public int LongBreakDuration {
        get => _longBreakDuration;
        private init {
            if (value > 24 * 60)
                throw new Exception("Некорректное значение");
            _longBreakDuration = value;
        }}
    
    private readonly int _startTime = 480;
    public int StartTime {
        get => _startTime;
        private init {
            if (value > 24 * 60)
                throw new Exception("Некорректное значение");
            _startTime = value;
        }
    }

    public Settings() {}

    [JsonConstructor]
    public Settings(int lessonDuration, int breakDuration, int longBreakDuration, int startTime, string adminPassword) {
        AdminPassword = adminPassword;
        LessonDuration = lessonDuration;
        BreakDuration = breakDuration;
        LongBreakDuration = longBreakDuration;
        StartTime = startTime;
    }
}