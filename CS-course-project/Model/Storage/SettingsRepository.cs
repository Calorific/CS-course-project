using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Storage; 

public class SettingsRepository : IRepository<ISettings, ISettings, bool> {
    private const string Path = "./data/settings.json";
    private ISettings? _settings;
    
    public async Task<bool> Update(ISettings newItem) {
        return await Task.Run(() => {
            try {
                _settings = newItem;
                var json = JsonSerializer.Serialize(newItem);
                File.WriteAllText(Path, json);
                return true;
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
        });
    }

    public async Task<ISettings> GetData() {
        return await Task.Run(() => {
            if (_settings != null) return _settings;
            var json = File.ReadAllText(Path);
            if (json.Length == 0)
                return new Settings();
            var data = JsonSerializer.Deserialize<Settings>(json);
            _settings = data;
            return data ?? new Settings();
        });
    }

    public async Task<bool> RemoveAt(bool key) {
        return await Task.Run(() => {
            try {
                if (!key) return false;
                File.WriteAllText(Path, "");
                _settings = null;
                return true;
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
        });
    }

    public SettingsRepository() {
        if (!Directory.Exists("./data"))
            Directory.CreateDirectory("./data");
        
        if (!File.Exists(Path)) 
            File.Create(Path).Dispose();
    }
}