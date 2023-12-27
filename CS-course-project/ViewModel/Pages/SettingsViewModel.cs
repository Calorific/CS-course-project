using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CS_course_project.Model.Storage;
using CS_course_project.Model.Timetables;
using CS_course_project.ViewModel.Commands;
using CS_course_project.ViewModel.Common;
using CS_course_project.ViewModel.Components.Common;

namespace CS_course_project.ViewModel.Pages;

public class SettingsViewModel : NotifyErrorsViewModel {
    private readonly INavigator? _navigator;
    private readonly IDataManager? _dataManager;
    
    public ICommand GoBackCommand => Command.Create(GoBack);
    private void GoBack(object? sender, EventArgs e) {
        _navigator?.Navigate(Routes.AdminPanel);
    }
    
    #region AddCommands
    
    public ICommand AddGroupCommand => Command.Create(AddGroup);
    private async void AddGroup(object? sender, EventArgs e) {
        if (sender is not string item || _dataManager == null) return;
        InsertSort(Groups, item);
        await _dataManager.UpdateGroups(item);
    }
    
    public ICommand AddTeacherCommand => Command.Create(AddTeacher);
    private async void AddTeacher(object? sender, EventArgs e) {
        if (sender is not string name || _dataManager == null) return;
        var newTeacher = new Teacher(name);
        InsertTeacher(Teachers, newTeacher);
        await _dataManager.UpdateTeachers(newTeacher);
    }
    
    public ICommand AddClassroomCommand => Command.Create(AddClassroom);
    private async void AddClassroom(object? sender, EventArgs e) {
        if (sender is not string item || _dataManager == null) return;
        InsertSort(Classrooms, item);
        await _dataManager.UpdateClassrooms(item);
    }
    
    public ICommand AddSubjectCommand => Command.Create(AddSubject);
    private async void AddSubject(object? sender, EventArgs e) {
        if (sender is not string item || _dataManager == null) return;
        InsertSort(Subjects, item);
        await _dataManager.UpdateSubjects(item);
    }
    
    #endregion
    
    
    # region RemoveCommands
    
    public ICommand RemoveGroupCommand => Command.Create(RemoveGroup);
    private async void RemoveGroup(object? sender, EventArgs e) {
        if (sender is not string id || _dataManager == null) 
            return;
        
        Groups.Remove(Groups.First(g => g.Id == id));
        await _dataManager.RemoveGroup(id);
    }
    
    public ICommand RemoveTeacherCommand => Command.Create(RemoveTeacher);
    private async void RemoveTeacher(object? sender, EventArgs e) {
        if (sender is not string id || _dataManager == null) 
            return;
        
        foreach (var listItem in Teachers) {
            listItem.Error = null;
        }
        
        for (var i = 0; i < Teachers.Count; i++)
            if (Teachers[i].Id == id) {
                try {
                    await _dataManager.RemoveTeacher(id);
                    Teachers.RemoveAt(i);
                }
                catch (ArgumentException exception) {
                    Teachers[i].Error = exception.Message;
                }
                
                break;
            }
    }
    
    public ICommand RemoveClassroomCommand => Command.Create(RemoveClassroom);
    private async void RemoveClassroom(object? sender, EventArgs e) {
        if (sender is not string id || _dataManager == null)
            return;
        
        foreach (var listItem in Classrooms) {
            listItem.Error = null;
        }

        try {
            await _dataManager.RemoveClassroom(id);
            Classrooms.Remove(Classrooms.First(c => c.Id == id));
        }
        catch (ArgumentException exception) {
            Classrooms.First(c => c.Id == id).Error = exception.Message;
        }
    }
    
    public ICommand RemoveSubjectCommand => Command.Create(RemoveSubject);
    private async void RemoveSubject(object? sender, EventArgs e) {
        if (sender is not string id || _dataManager == null) 
            return;
        
        foreach (var listItem in Subjects) {
            listItem.Error = null;
        }

        try {
            await _dataManager.RemoveSubject(id);
            Subjects.Remove(Subjects.First(c => c.Id == id));
        }
        catch (ArgumentException exception) {
            Subjects.First(s => s.Id == id).Error = exception.Message;
        } 
    }

    #endregion


    #region Collections

    private ObservableCollection<ListItem> _groups = new();   
    public ObservableCollection<ListItem> Groups {
        
        get => _groups; 
        set {
            if (_groups == value) return;
            _groups = value;
            Notify();
        }
    }
    
    private ObservableCollection<ListItem> _teachers = new();
    public ObservableCollection<ListItem> Teachers {
        get => _teachers; 
        set {
            if (_teachers == value) return;
            _teachers = value;
            Notify();
        }
    }
    
    private ObservableCollection<ListItem> _classrooms = new();
    public ObservableCollection<ListItem> Classrooms {
        get => _classrooms; 
        set {
            if (_classrooms == value) return;
            _classrooms = value;
            Notify();
        }
    }
    
    private ObservableCollection<ListItem> _subjects = new();
    public ObservableCollection<ListItem> Subjects {
        get => _subjects; 
        set {
            if (_subjects == value) return;
            _subjects = value;
            Notify();
        }
    }

    #endregion

    private static void InsertSort(IList<ListItem> collection, string newItem) {
        for (var i = 0; i < collection.Count; i++) {
            if (string.Compare(collection[i].Data, newItem, StringComparison.OrdinalIgnoreCase) <= 0) continue;
            collection.Insert(i, new ListItem(newItem));
            return;
        }
        collection.Add(new ListItem(newItem));
    }
    
    private static void InsertTeacher(IList<ListItem> collection, ITeacher newTeacher) {
        var newItem = new ListItem(newTeacher.Name, newTeacher.Id);
        for (var i = 0; i < collection.Count; i++) {
            if (string.Compare(collection[i].Data, newItem.Data, StringComparison.CurrentCultureIgnoreCase) <= 0) continue;
            collection.Insert(i, newItem);
            return;
        }
        collection.Add(newItem);
    }
    
    private async void Init() {
        if (_dataManager == null) return;
        var groups = (await _dataManager.GetGroups()).OrderBy(g => g.ToLower());
        Groups = new ObservableCollection<ListItem>(groups.Select(group => new ListItem(group)));
        Groups.CollectionChanged += (_, _) => Notify(nameof(Groups));
        
        var teachers = (await _dataManager.GetTeachers()).OrderBy(t => t.Name.ToLower());
        Teachers = new ObservableCollection<ListItem>(teachers.Select(teacher => new ListItem(teacher.Name, teacher.Id)));
        Teachers.CollectionChanged += (_, _) => Notify(nameof(Teachers));
        
        var classrooms = (await _dataManager.GetClassrooms()).OrderBy(t => t.ToLower());
        Classrooms = new ObservableCollection<ListItem>(classrooms.Select(classroom => new ListItem(classroom)));
        Classrooms.CollectionChanged += (_, _) => Notify(nameof(Classrooms));
        
        var subjects = (await _dataManager.GetSubjects()).OrderBy(t => t.ToLower());
        Subjects = new ObservableCollection<ListItem>(subjects.Select(subject => new ListItem(subject)));
        Subjects.CollectionChanged += (_, _) => Notify(nameof(Subjects));
    }
    
    public SettingsViewModel(INavigator navigator, IDataManager dataManager) {
        _navigator = navigator;
        _dataManager = dataManager;
        
        Init();
    }

    public SettingsViewModel() {}
}