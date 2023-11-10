using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CS_course_project.model.Storage; 

public enum RepositoryItems { Groups, Teachers, Classrooms, Subjects }; 

public class BaseRepository : IRepository<string, List<string>, int> {
    private readonly string _path = "./data/";

    private void SaveItems(List<string> groups) {
        var json = JsonSerializer.Serialize(groups);
        File.WriteAllText(_path, json);
    }
    
    public async void Update(string newGroup) {
        await Task.Run(async () => {
            var groups = await GetData();
            groups.Add(newGroup);
            SaveItems(groups);
        });
    }

    public async Task<List<string>> GetData() {
        return await Task.Run(() => {
            var json = File.ReadAllText(_path);
            if (json.Length == 0)
                return new List<string>();
            var data = JsonSerializer.Deserialize<List<string>>(json);
            return data ?? new List<string>();
        });
    }

    public async void RemoveAt(int key) {
        await Task.Run(async () => {
            var groups = await GetData();
            if (key > 0 && key < groups.Count)
                groups.RemoveAt(key);
            SaveItems(groups);
        });
    }

    public BaseRepository(RepositoryItems type) {
        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);
        
        _path += type switch {
            RepositoryItems.Groups => "groups.json",
            RepositoryItems.Classrooms => "classrooms.json",
            RepositoryItems.Teachers => "teachers.json",
            RepositoryItems.Subjects => "subjects.json",
            _ => "error.json"
        };
        
        if (!File.Exists(_path)) 
            File.Create(_path).Dispose();
    }
}