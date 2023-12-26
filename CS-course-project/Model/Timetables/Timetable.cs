using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CS_course_project.Model.Timetables; 

public class Timetable : ITimetable {

    public string Group { get; }
    public IList<IDay> Days { get; }

    [JsonConstructor]
    public Timetable(string group, IList<IDay> days) {
        if (days == null || days.Count == 0)
            throw new ArgumentException("Список дней не может быть пустым");
        if (string.IsNullOrEmpty(group))
            throw new ArgumentException("Группа не может быть пустой");
        Group = group;
        Days = days;
    }
}