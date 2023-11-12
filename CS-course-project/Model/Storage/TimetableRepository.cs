using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CS_course_project.model.Timetables; 

namespace CS_course_project.model.Storage; 

public class TimetableRepository : IRepository<Timetable, Dictionary<string, Timetable>, string> {
    private const string Path = "./data/timetables.json";
    
    private static void SaveItems(Dictionary<string, Timetable> timetables) {
        var json = JsonSerializer.Serialize(timetables);
        File.WriteAllText(Path, json);
    }

    public async Task<bool> Update(Timetable newItem) {
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

    public async Task<Dictionary<string, Timetable>> GetData() {
        return await Task.Run(() => {
            var json = File.ReadAllText(Path);
            if (json.Length == 0)
                return new Dictionary<string, Timetable>();
            var data = JsonSerializer.Deserialize<Dictionary<string, Timetable>>(json);
            return data ?? new Dictionary<string, Timetable>();
        });
    }

    public async Task<bool> RemoveAt(string key) {
        return await Task.Run(() => {
            try {
                var json = File.ReadAllText(Path);
                if (json.Length == 0)
                    return false;
                var data = JsonSerializer.Deserialize<Dictionary<string, Timetable>>(json);
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