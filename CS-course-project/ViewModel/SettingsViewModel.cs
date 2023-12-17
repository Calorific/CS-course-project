using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CS_course_project.Commands;
using CS_course_project.model.Storage;
using CS_course_project.Model.Timetables;
using CS_course_project.Navigation;
using CS_course_project.UserControls.SettingsPage;

namespace CS_course_project.ViewModel;

public class SettingsViewModel : NotifyErrorsViewModel {
    
    public static ICommand GoBackCommand => Command.Create(GoBack);
    private static void GoBack(object? sender, EventArgs e) {
        Navigator.Navigate.Execute("AdminPanel", null);
    }
    
    #region AddCommands
    
    public ICommand AddGroupCommand => Command.Create(AddGroup);
    private async void AddGroup(object? sender, EventArgs e) {
        if (sender is not string item) return;
        InsertSort(Groups, item);
        await DataManager.UpdateGroups(item);
    }
    
    public ICommand AddTeacherCommand => Command.Create(AddTeacher);
    private async void AddTeacher(object? sender, EventArgs e) {
        if (sender is not string name) return;
        var newTeacher = new Teacher(name);
        InsertTeacher(Teachers, newTeacher);
        await DataManager.UpdateTeachers(newTeacher);
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
        if (sender is not string id) return;
        Groups.Remove(id);
        await DataManager.RemoveGroup(id);
    }
    
    public ICommand RemoveTeacherCommand => Command.Create(RemoveTeacher);
    private async void RemoveTeacher(object? sender, EventArgs e) {
        if (sender is not string id) return;
        for (var i = 0; i < Teachers.Count; i++)
            if (Teachers[i].Id == id) {
                Teachers.RemoveAt(i);
                await DataManager.RemoveTeacher(id);
                break;
            }
    }
    
    public ICommand RemoveClassroomCommand => Command.Create(RemoveClassroom);
    private async void RemoveClassroom(object? sender, EventArgs e) {
        if (sender is not string id) return;
        Classrooms.Remove(id);
        await DataManager.RemoveClassroom(id);
    }
    
    public ICommand RemoveSubjectCommand => Command.Create(RemoveSubject);
    private async void RemoveSubject(object? sender, EventArgs e) {
        if (sender is not string id) return;
        Subjects.Remove(id);
        await DataManager.RemoveSubject(id);
    }

    #endregion


    #region Collections

    private ObservableCollection<string> _groups;

    public ObservableCollection<Item> GroupItems =>
        new(Groups.Select(group => new Item(group)).OrderBy(s => s.Data.ToLower()));
    public ObservableCollection<string> Groups {
        
        get => _groups; 
        set {
            if (_groups == value) return;
            _groups = value;
            NotifyAll(nameof(Groups), nameof(GroupItems));
        }
    }
    
    private ObservableCollection<ITeacher> _teachers;
    public ObservableCollection<Item> TeacherItems => 
        new(Teachers.Select(teacher => new Item(teacher.Name, teacher.Id)).OrderBy(s => s.Data.ToLower()));
    private ObservableCollection<ITeacher> Teachers {
        get => _teachers; 
        set {
            if (_teachers == value) return;
            _teachers = value;
            NotifyAll(nameof(Teachers), nameof(TeacherItems));
        }
    }
    
    private ObservableCollection<string> _classrooms;
    public ObservableCollection<Item> ClassroomItems =>
        new(Classrooms.Select(classroom => new Item(classroom)).OrderBy(s => s.Data.ToLower()));
    public ObservableCollection<string> Classrooms {
        get => _classrooms; 
        set {
            if (_classrooms == value) return;
            _classrooms = value;
            NotifyAll(nameof(Classrooms), nameof(ClassroomItems));
        }
    }
    
    private ObservableCollection<string> _subjects;
    public ObservableCollection<Item> SubjectItems =>
        new(Subjects.Select(subject => new Item(subject)).OrderBy(s => s.Data.ToLower()));
    public ObservableCollection<string> Subjects {
        get => _subjects; 
        set {
            if (_subjects == value) return;
            _subjects = value;
            NotifyAll(nameof(Subjects), nameof(SubjectItems));
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
    
    private static void InsertTeacher(IList<ITeacher> collection, ITeacher newItem) {
        for (var i = 0; i < collection.Count; i++) {
            if (string.Compare(collection[i].Name, newItem.Name, StringComparison.CurrentCultureIgnoreCase) <= 0) continue;
            collection.Insert(i, newItem);
            return;
        }
        collection.Add(newItem);
    }
    
    private async void Init() {
        Groups = new ObservableCollection<string>(await DataManager.LoadGroups());
        Groups.CollectionChanged += (_, _) => Notify(nameof(GroupItems));
        
        Teachers = new ObservableCollection<ITeacher>((await DataManager.LoadTeachers()).OrderBy(s => s.Name.ToLower()));
        Teachers.CollectionChanged += (_, _) => Notify(nameof(TeacherItems));
        
        Classrooms = new ObservableCollection<string>(await DataManager.LoadClassrooms());
        Classrooms.CollectionChanged += (_, _) => Notify(nameof(ClassroomItems));
        
        Subjects = new ObservableCollection<string>(await DataManager.LoadSubjects());
        Subjects.CollectionChanged += (_, _) => Notify(nameof(SubjectItems));
    }
    
    public SettingsViewModel() {
        _groups = new ObservableCollection<string>();
        _classrooms = new ObservableCollection<string>();
        _subjects = new ObservableCollection<string>();
        _teachers = new ObservableCollection<ITeacher>();
        
        Init();
    }
}