using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CS_course_project.Model.Timetables;

namespace CS_course_project.model.Storage; 

public class TeacherRepository : IRepository<Teacher, List<Teacher>, int> {
    private const string Path = "./data/teachers.json";

    private static void SaveItems(List<Teacher> groups) {
        var json = JsonSerializer.Serialize(groups);
        File.WriteAllText(Path, json);
    }
    
    public async Task<bool> Update(Teacher newItem) {
        return await Task.Run(async () => {
            try {
                var teachers = await GetData();
                teachers.Add(newItem);
                SaveItems(teachers);
                return true;
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
        });
    }

    public async Task<List<Teacher>> GetData() {
        return await Task.Run(() => {
            var json = File.ReadAllText(Path);
            if (json.Length == 0)
                return new List<Teacher>();
            var data = JsonSerializer.Deserialize<List<Teacher>>(json);
            return data ?? new List<Teacher>();
        });
    }

    public async Task<bool> RemoveAt(int key) {
        return await Task.Run(async () => {
            try {
                var teachers = await GetData();
                if (key < 0 || key >= teachers.Count) return false;
                teachers.RemoveAt(key);
                SaveItems(teachers);
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