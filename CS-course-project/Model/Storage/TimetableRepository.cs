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
    private Dictionary<string, ITimetable>? _data;
    
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
            if (_data != null) return _data;
            var json = File.ReadAllText(Path);
            if (json.Length == 0)
                return new Dictionary<string, ITimetable>();
            var jsonData = JsonSerializer.Deserialize<Dictionary<string, Timetable>>(json);
            var data = jsonData?.Keys.ToDictionary<string?, string, ITimetable>(key => key, key => jsonData[key]);
            _data = data;
            return data ?? new Dictionary<string, ITimetable>();
        });
    }

    public async Task<bool> RemoveAt(string key) {
        return await Task.Run(async () => {
            try {
                var data = await GetData();
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