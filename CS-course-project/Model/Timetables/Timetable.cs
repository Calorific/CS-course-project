using System.Collections.Generic;
using System.Text.Json.Serialization;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Timetables; 

public class Timetable : ITimetable {
    public string Group { get; }

    public IList<IDay> Days { get; }

    [JsonConstructor]
    public Timetable(string group, IList<IDay> days) {
        Group = group;
        Days = days;
    }
}