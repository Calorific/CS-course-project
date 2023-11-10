using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Storage; 

public class SettingsRepository : IRepository<Settings, Settings, bool> {
    private const string Path = "./data/settings.json";
    
    public async void Update(Settings newItem) {
        await Task.Run(() => {
            var json = JsonSerializer.Serialize(newItem);
            File.WriteAllText(Path, json);
        });
    }

    public async Task<Settings> GetData() {
        return await Task.Run(() => { 
            var json = File.ReadAllText(Path);
            if (json.Length == 0)
                return new Settings();
            var data = JsonSerializer.Deserialize<Settings>(json);
            return data ?? new Settings();
        });
    }

    public async void RemoveAt(bool key) {
        await Task.Run(() => {
            if (key)
                File.WriteAllText(Path, "");
        });
    }

    public SettingsRepository() {
        if (!Directory.Exists("./data"))
            Directory.CreateDirectory("./data");
        
        if (!File.Exists(Path)) 
            File.Create(Path).Dispose();
    }
}