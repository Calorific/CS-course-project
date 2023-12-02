﻿using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Timetables; 

public class Timetable : ITimetable {
    public string Group { get; } = "";

    public IList<IDay> Days { get; } = Enumerable.Range(0, 7).Select(idx => new Day(idx, null) as IDay).ToList();
    
    public Timetable() {}

    [JsonConstructor]
    public Timetable(string group, IList<IDay> days) {
        Group = group;
        Days = days;
    }
}