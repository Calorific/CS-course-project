namespace CS_course_project.model; 

public class Settings {
    public static Settings? Self;

    public int LessonDuration;
    public int BreakDuration;
    public int LongBreakDuration;
    public int StartTime;

    public Settings(int lessonDuration, int breakDuration, int longBreakDuration, int startTime) {
        LessonDuration = 0;
        Self ??= new Settings(lessonDuration, breakDuration, longBreakDuration, startTime);
        LessonDuration = lessonDuration;
        BreakDuration = breakDuration;
        LongBreakDuration = longBreakDuration;
        StartTime = startTime;
    }
}