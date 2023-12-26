using System;
using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CS_course_project.Model.Services.AuthServices;

namespace CS_course_project.Model.Storage;


public class SessionRepository : IRepository<ISession, ISession?, bool> {
    private static readonly string Path = ConfigurationManager.AppSettings["StoragePath"]! + "session.json";

    private static void CheckPath() {
        if (!Directory.Exists(ConfigurationManager.AppSettings["StoragePath"]!))
            Directory.CreateDirectory(ConfigurationManager.AppSettings["StoragePath"]!);
        if (!File.Exists(Path)) 
            File.Create(Path).Dispose();
    }
    
    public async Task<bool> Update(ISession newItem) {
        CheckPath();
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

    public async Task<ISession?> Read() {
        CheckPath();
        return await Task.Run(() => { 
            var json = File.ReadAllText(Path);
            if (json.Length == 0)
                return null;
            var data = JsonSerializer.Deserialize<Session>(json);
            return data as ISession ?? null;
        });
    }

    public async Task<bool> Delete(bool key) {
        CheckPath();
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
        CheckPath();
    }
}