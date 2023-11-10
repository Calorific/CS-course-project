﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CS_course_project.model.Storage;
using CS_course_project.Navigation;

namespace CS_course_project.ViewModel; 

public class AdminPanelViewModel : NotifyErrorsViewModel {
    private List<string>? _groups;
    private List<string>? _teachers;
    private List<string>? _classrooms;
    private List<string>? _subjects;

    private async void ShouldRedirect() {
        _groups = await DataManager.LoadGroups();
        _teachers = await DataManager.LoadTeachers();
        _classrooms = await DataManager.LoadClassrooms();
        _subjects = await DataManager.LoadSubjects();
        if (_groups?.Count == 0 || _teachers?.Count == 0 || _classrooms?.Count == 0 || _subjects?.Count == 0) {
            Navigator.Navigate.Execute("Settings", null);
        }
    }
    
    public AdminPanelViewModel() {
        ShouldRedirect();
    }
}