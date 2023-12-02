using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CS_course_project.model.Timetables;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Storage; 

public class TimetableRepository : IRepository<ITimetable, Dictionary<string, ITimetable>, string> {
    private const string Path = "./data/timetables.json";
    
    private static void SaveItems(Dictionary<string, ITimetable> timetables) {
        var json = JsonSerializer.Serialize(timetables);
        File.WriteAllText(Path, json);
    }

    public async Task<bool> Update(ITimetable newItem) {
        return await Task.Run(async () => {
            try {
                var timetables = await GetData();
                timetables[newItem.Group] = newItem;
                SaveItems(timetables);
                return true;
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
        });
    }

    public async Task<Dictionary<string, ITimetable>> GetData() {
        return await Task.Run(() => {
            var json = File.ReadAllText(Path);
            if (json.Length == 0)
                return new Dictionary<string, ITimetable>();
            var data = JsonSerializer.Deserialize<Dictionary<string, Timetable>>(json);
            return data == null ? new Dictionary<string, ITimetable>() : data.Keys.ToDictionary<string?, string, ITimetable>(key => key, key => data[key]);
        });
    }

    public async Task<bool> RemoveAt(string key) {
        return await Task.Run(() => {
            try {
                var json = File.ReadAllText(Path);
                if (json.Length == 0)
                    return false;
                var data = JsonSerializer.Deserialize<Dictionary<string, ITimetable>>(json);
                if (data == null) return false;
                data.Remove(key);
                SaveItems(data);
                return true;
            } 
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
        });
    }
    
    public TimetableRepository() {
        if (!Directory.Exists("./data"))
            Directory.CreateDirectory("./data");
        
        if (!File.Exists(Path)) 
            File.Create(Path).Dispose();
    }
}