using System.Collections.Generic;
using System.Threading.Tasks;
using CS_course_project.model.Storage;
using CS_course_project.Navigation;

namespace CS_course_project.ViewModel; 

public class AdminPanelViewModel : NotifyErrorsViewModel {
    private List<string>? _groups;
    private async Task LoadGroups() => _groups = await DataManager.GroupRepository.GetData();
    
    private List<string>? _teachers;   
    private async Task LoadTeachers() => _teachers = await DataManager.TeachersRepository.GetData();
    
    private List<string>? _classrooms;
    private async Task LoadClassrooms() => _classrooms = await DataManager.ClassroomsRepository.GetData();
    
    private List<string>? _subjects;
    private async Task LoadSubjects() => _subjects = await DataManager.SubjectsRepository.GetData();

    private async void ShouldRedirect() {
        await LoadGroups();
        await LoadTeachers();
        await LoadClassrooms();
        await LoadSubjects();
        if (_groups?.Count == 0 || _teachers?.Count == 0 || _classrooms?.Count == 0 || _subjects?.Count == 0) {
            Navigator.Navigate.Execute("Settings", null);
        }
    }
    
    public AdminPanelViewModel() {
        ShouldRedirect();
    }
}