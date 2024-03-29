﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CS_course_project.Model.Timetables;

namespace CS_course_project.Model.Storage; 

public class TeachersRepository : IRepository<ITeacher, List<ITeacher>, string> {
    private static readonly string Path = (ConfigurationManager.AppSettings["StoragePath"] ?? "./data/") + "teachers.json";
    private List<ITeacher>? _data;

    private static void CheckPath() {
        if (!Directory.Exists(ConfigurationManager.AppSettings["StoragePath"] ?? "./data/"))
            Directory.CreateDirectory(ConfigurationManager.AppSettings["StoragePath"] ?? "./data/");
        if (!File.Exists(Path)) 
            File.Create(Path).Dispose();
    }
    
    private static void SaveItems(List<ITeacher> teachers) {
        CheckPath();
        var json = JsonSerializer.Serialize(teachers);
        File.WriteAllText(Path, json);
    }
    
    public async Task<bool> Update(ITeacher newItem) {
        return await Task.Run(async () => {
            try {
                var teachers = await Read();
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

    public async Task<List<ITeacher>> Read() {
        CheckPath();
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

    public async Task<bool> Delete(string id) {
        return await Task.Run(async () => {
            try {
                var teachers = await Read();
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
    
    public TeachersRepository() {
        CheckPath();
    }
}