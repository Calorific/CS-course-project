using System.IO;
using System.Text.Json;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Storage; 

public class SettingsRepository : IRepository<Settings, Settings, bool> {
    private const string Path = "./data/settings.json";
    
    public void Update(Settings newItem) {
        var json = JsonSerializer.Serialize(newItem);
        File.WriteAllText(Path, json);
    }

    public Settings GetData() {
        var json = File.ReadAllText(Path);
        if (json.Length == 0)
            return new Settings();
        var data = JsonSerializer.Deserialize<Settings>(json);
        return data ?? new Settings();
    }

    public void RemoveAt(bool key) {
        if (key) File.WriteAllText(Path, "");
    }
}