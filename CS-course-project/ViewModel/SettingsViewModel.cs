using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.model.Storage;

namespace CS_course_project.ViewModel;

public class SettingsViewModel : NotifyErrorsViewModel {
    
    #region AddCommands
    
    public ICommand AddGroupCommand => Command.Create(AddGroup);
    private async void AddGroup(object? sender, EventArgs e) {
        if (sender is not string item) return;
        InsertSort(Groups, item);
        await DataManager.UpdateGroups(item);
    }
    
    public ICommand AddTeacherCommand => Command.Create(AddTeacher);
    private async void AddTeacher(object? sender, EventArgs e) {
        if (sender is not string item) return;
        InsertSort(Teachers, item);
        await DataManager.UpdateTeachers(item);
    }
    
    public ICommand AddClassroomCommand => Command.Create(AddClassroom);
    private async void AddClassroom(object? sender, EventArgs e) {
        if (sender is not string item) return;
        InsertSort(Classrooms, item);
        await DataManager.UpdateClassrooms(item);
    }
    
    public ICommand AddSubjectCommand => Command.Create(AddSubject);
    private async void AddSubject(object? sender, EventArgs e) {
        if (sender is not string item) return;
        InsertSort(Subjects, item);
        await DataManager.UpdateSubjects(item);
    }
    
    #endregion
    
    
    # region RemoveCommands
    
    public ICommand RemoveGroupCommand => Command.Create(RemoveGroup);
    private async void RemoveGroup(object? sender, EventArgs e) {
        if (sender is not string item) return;
        var idx = Groups.IndexOf(item);
        Groups.Remove(item);
        await DataManager.GroupsRemoveAt(idx);
    }
    
    public ICommand RemoveTeacherCommand => Command.Create(RemoveTeacher);
    private async void RemoveTeacher(object? sender, EventArgs e) {
        if (sender is not string item) return;
        var idx = Teachers.IndexOf(item);
        Teachers.Remove(item);
        await DataManager.TeachersRemoveAt(idx);
    }
    
    public ICommand RemoveClassroomCommand => Command.Create(RemoveClassroom);
    private async void RemoveClassroom(object? sender, EventArgs e) {
        if (sender is not string item) return;
        var idx = Classrooms.IndexOf(item);
        Classrooms.Remove(item);
        await DataManager.ClassroomsRemoveAt(idx);
    }
    
    public ICommand RemoveSubjectCommand => Command.Create(RemoveSubject);
    private async void RemoveSubject(object? sender, EventArgs e) {
        if (sender is not string item) return;
        var idx = Subjects.IndexOf(item);
        Subjects.Remove(item);
        await DataManager.SubjectsRemoveAt(idx);
    }

    #endregion


    #region Collections

    private ObservableCollection<string> _groups;
    public ObservableCollection<string> Groups {
        get => _groups; 
        set {
            if (_groups == value) return;
            _groups = value;
            Notify();
        }
    }
    
    private ObservableCollection<string> _teachers;
    public ObservableCollection<string> Teachers {
        get => _teachers; 
        set {
            if (_teachers == value) return;
            _teachers = value;
            Notify();
        }
    }
    
    private ObservableCollection<string> _classrooms;
    public ObservableCollection<string> Classrooms {
        get => _classrooms; 
        set {
            if (_classrooms == value) return;
            _classrooms = value;
            Notify();
        }
    }
    
    private ObservableCollection<string> _subjects;
    public ObservableCollection<string> Subjects {
        get => _subjects; 
        set {
            if (_subjects == value) return;
            _subjects = value;
            Notify();
        }
    }

    #endregion
    

    private static void InsertSort(IList<string> collection, string newItem) {
        for (var i = 0; i < collection.Count; i++) {
            if (string.Compare(collection[i], newItem, StringComparison.OrdinalIgnoreCase) <= 0) continue;
            collection.Insert(i, newItem);
            return;
        }
        collection.Add(newItem);
    }
    
    private async void Init() {
        var groups = (await DataManager.LoadGroups()).OrderBy(s => s.ToLower());
        Groups = new ObservableCollection<string>(groups);
        
        var teachers = (await DataManager.LoadTeachers()).OrderBy(s => s.ToLower());
        Teachers = new ObservableCollection<string>(teachers);

        var classrooms = (await DataManager.LoadClassrooms()).OrderBy(s => s.ToLower());
        Classrooms = new ObservableCollection<string>(classrooms);
        
        var subjects = (await DataManager.LoadSubjects()).OrderBy(s => s.ToLower());
        Subjects = new ObservableCollection<string>(subjects);
    }
    
    public SettingsViewModel() {
        _groups = new ObservableCollection<string>();
        _teachers = new ObservableCollection<string>();
        _classrooms = new ObservableCollection<string>();
        _subjects = new ObservableCollection<string>();
        Init();
    }
}