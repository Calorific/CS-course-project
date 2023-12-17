using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CS_course_project.model.Timetables;
using CS_course_project.Model.Timetables;
using Newtonsoft.Json;

namespace CS_course_project.model.Storage;

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

public class TimetableRepository : IRepository<ITimetable, Dictionary<string, ITimetable>, string> {
    private readonly JsonSerializerSettings _jsonSettings = new()
    {
        Converters = {
            new AbstractConverter<Timetable, ITimetable>(),
            new AbstractConverter<Day, IDay>(),
            new AbstractConverter<Lesson, ILesson>(),
        },
    };
    
    private const string Path = "./data/timetables.json";
    private Dictionary<string, ITimetable>? _data;

    private void SaveItems(Dictionary<string, ITimetable> timetables) {
        var json = JsonConvert.SerializeObject(timetables, _jsonSettings);
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
            var data = JsonConvert.DeserializeObject<Dictionary<string, ITimetable>>(json, _jsonSettings);
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