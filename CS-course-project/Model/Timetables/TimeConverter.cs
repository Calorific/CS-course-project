namespace CS_course_project.Model.Timetables; 

public class TimeConverter {
    public static int ParseTime(string time) {
        var parts = time.Split(':');
        var res = 0;
        
        if (int.TryParse(parts[0], out var hours))
            res += hours * 60;
        if (parts.Length > 1 && int.TryParse(parts[1], out var minutes))
            res += minutes;
        return res;
    }

    public static string FormatTime(int time) {
        var minutes = time % 60;
        return (time / 60).ToString() + ':' + (minutes < 10 ? "0" : "") + minutes;
    }
}