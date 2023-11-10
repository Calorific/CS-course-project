namespace CS_course_project.Model.Timetables; 

public class Settings {
    public int LessonDuration { get; } = 40;
    public int BreakDuration { get; } = 10;
    public int LongBreakDuration { get; } = 15;
    public int StartTime { get; } = 480;
    
    public Settings() {}

    public Settings(int lessonDuration, int breakDuration, int longBreakDuration, int startTime) {
        LessonDuration = 0;
        LessonDuration = lessonDuration;
        BreakDuration = breakDuration;
        LongBreakDuration = longBreakDuration;
        StartTime = startTime;
    }
}