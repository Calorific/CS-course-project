using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Storage; 

public class TeacherRepository : IRepository<ITeacher, List<ITeacher>, string> {
    private const string Path = "./data/teachers.json";
    private List<ITeacher>? _data;

    private static void SaveItems(List<ITeacher> groups) {
        var json = JsonSerializer.Serialize(groups);
        File.WriteAllText(Path, json);
    }
    
    public async Task<bool> Update(ITeacher newItem) {
        return await Task.Run(async () => {
            try {
                var teachers = await GetData();
                teachers.Add(newItem);
                SaveItems(teachers);
                _data = teachers;
                return true;
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
        });
    }

    public async Task<List<ITeacher>> GetData() {
        return await Task.Run(() => {
            if (_data != null) return _data;
            var json = File.ReadAllText(Path);
            if (json.Length == 0)
                return new List<ITeacher>();
            var data = JsonSerializer.Deserialize<List<Teacher>>(json)?.Cast<ITeacher>().ToList();
            _data = data;
            return data ?? new List<ITeacher>();
        });
    }

    public async Task<bool> RemoveAt(string id) {
        return await Task.Run(async () => {
            try {
                var teachers = await GetData();
                var idx = teachers.FindIndex(t => t.Id == id);
                teachers.RemoveAt(idx);
                SaveItems(teachers);
                _data = teachers;
                return true;
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
        });
    }
    
    public TeacherRepository() {
        if (!Directory.Exists("./data"))
            Directory.CreateDirectory("./data");
        
        if (!File.Exists(Path)) 
            File.Create(Path).Dispose();
    }
}