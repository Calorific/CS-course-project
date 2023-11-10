using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CS_course_project.model.Timetables; 

namespace CS_course_project.model.Storage; 

public class TimetableRepository : IRepository<Timetable, Dictionary<string, Timetable>, string> {
    private const string Path = "./data/timetables.json";
    
    private static void SaveItems(Dictionary<string, Timetable> timetables) {
        var json = JsonSerializer.Serialize(timetables);
        File.WriteAllText(Path, json);
    }

    public void Update(Timetable newItem) {
        var timetables = GetData();
        timetables[newItem.Group] = newItem;
        SaveItems(timetables);
    }

    public Dictionary<string, Timetable> GetData() {
        var json = File.ReadAllText(Path);
        if (json.Length == 0)
            return new Dictionary<string, Timetable>();
        var data = JsonSerializer.Deserialize<Dictionary<string, Timetable>>(json);
        return data ?? new Dictionary<string, Timetable>();
    }

    public void RemoveAt(string key) {
        var json = File.ReadAllText(Path);
        if (json.Length == 0)
            return;
        var data = JsonSerializer.Deserialize<Dictionary<string, Timetable>>(json);
        if (data == null) return;
        data.Remove(key);
        SaveItems(data);
    }
}