using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using CS_course_project.Model.Timetables;
using Newtonsoft.Json;

namespace CS_course_project.Model.Storage;

public class AbstractConverter<TReal, TAbstract> 
    : JsonConverter where TReal : TAbstract 
{
    public override bool CanConvert(Type objectType)
        => objectType == typeof(TAbstract);

    public override object? ReadJson(JsonReader reader, Type type, object? value, JsonSerializer jser) 
        => jser.Deserialize<TReal>(reader);

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer jser)
        => jser.Serialize(writer, value);
}

public class TimetablesRepository : IRepository<ITimetable, Dictionary<string, ITimetable>, string> {
    private readonly JsonSerializerSettings _jsonSettings = new()
    {
        Converters = {
            new AbstractConverter<Timetable, ITimetable>(),
            new AbstractConverter<Day, IDay>(),
            new AbstractConverter<Lesson, ILesson>(),
        },
    };
    
    private static readonly string Path = (ConfigurationManager.AppSettings["StoragePath"] ?? "./data/") + "timetables.json";
    private Dictionary<string, ITimetable>? _data;
    
    private static void CheckPath() {
        if (!Directory.Exists(ConfigurationManager.AppSettings["StoragePath"] ?? "./data/"))
            Directory.CreateDirectory(ConfigurationManager.AppSettings["StoragePath"] ?? "./data/");
        if (!File.Exists(Path)) 
            File.Create(Path).Dispose();
    }

    private void SaveItems(Dictionary<string, ITimetable> timetables) {
        CheckPath();
        var json = JsonConvert.SerializeObject(timetables, _jsonSettings);
        File.WriteAllText(Path, json);
    }

    public async Task<bool> Update(ITimetable newItem) {
        return await Task.Run(async () => {
            try {
                var timetables = await Read();
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

    public async Task<Dictionary<string, ITimetable>> Read() {
        CheckPath();
        return await Task.Run(() => {
            if (_data != null) return _data;
            var json = File.ReadAllText(Path);
            if (json.Length == 0)
                return new Dictionary<string, ITimetable>();
            var data = JsonConvert.DeserializeObject<Dictionary<string, ITimetable>>(json, _jsonSettings);
            _data = data;
            return data ?? new Dictionary<string, ITimetable>();
        });
    }

    public async Task<bool> Delete(string key) {
        return await Task.Run(async () => {
            try {
                var data = await Read();
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

    public TimetablesRepository() {
        CheckPath();
    }
}