using System;

using System.Text.RegularExpressions;

namespace CS_course_project.Model.Services.TimeServices; 

public partial class TimeConverter : ITimeConverter {
    [GeneratedRegex(@"\d\d?:\d\d?")]
    private static partial Regex TimeRegex();
    
    public int ParseTime(string time) {
        if (string.IsNullOrEmpty(time))
            throw new ArgumentException("Время не может быть пустым");
        if (!TimeRegex().IsMatch(time))
            throw new ArgumentException("Некорректный формат");
        
        var parts = time.Split(':');
        var res = 0;
        
        if (int.TryParse(parts[0], out var hours))
            res += hours * 60;
        if (parts.Length > 1 && int.TryParse(parts[1], out var minutes))
            res += minutes;
        return res;
    }

    public string FormatTime(int time) {
        if (time < 0) {
            throw new ArgumentException("Время не может быть отрицательным");
        }
        
        var minutes = time % 60;
        return (time / 60).ToString() + ':' + (minutes < 10 ? "0" : "") + minutes;
    }
}