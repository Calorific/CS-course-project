using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CS_course_project.model.Storage;

public class Session {
    public bool IsAdmin { get; set; }
    public string? Data { get; set; }

    public Session(bool isAdmin, string data) {
        IsAdmin = isAdmin;
        Data = data;
    }
    
    public Session() {}
}

public class SessionRepository : IRepository<Session, Session, bool> {
    private const string Path = "./data/session.json";
    
    public async Task<bool> Update(Session newItem) {
        return await Task.Run(() => {
            try {
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

    public async Task<Session> GetData() {
        return await Task.Run(() => { 
            var json = File.ReadAllText(Path);
            if (json.Length == 0)
                return new Session();
            var data = JsonSerializer.Deserialize<Session>(json);
            return data ?? new Session();
        });
    }

    public async Task<bool> RemoveAt(bool key) {
        return await Task.Run(() => {
            try {
                if (!key) return false;
                File.WriteAllText(Path, "");
                return true;
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
        });
    }
    
    public SessionRepository() {
        if (!Directory.Exists("./data"))
            Directory.CreateDirectory("./data");
        
        if (!File.Exists(Path)) 
            File.Create(Path).Dispose();
    }
}