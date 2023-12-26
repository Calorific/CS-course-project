using System.Collections.Generic;

namespace CS_course_project.ViewModel.Components.TimetableForm; 

public class DayViewModel {
    public IList<LessonViewModel> Lessons { get; }
    
    public string Name { get; }
    
    public DayViewModel(IList<LessonViewModel> lessons, string name) {
        Lessons = lessons;
        Name = name;
    }
}