using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Timetables; 

public class Timetable {
    public string Group { get; set; } = "";

    public List<Day> Days { get; } = Enumerable.Range(0, 7).Select(idx => new Day(idx, null)).ToList();
    
    public Timetable() {}

    [JsonConstructor]
    public Timetable(string group, List<Day> days) {
        Group = group;
        Days = days;
    }
}