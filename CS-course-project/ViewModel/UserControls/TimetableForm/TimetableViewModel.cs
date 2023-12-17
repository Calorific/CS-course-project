using System.Collections.Generic;

namespace CS_course_project.ViewModel.UserControls.TimetableForm; 

public class TimetableViewModel {
    public string Group { get; }
    public IList<DayViewModel> Days { get; }

    public TimetableViewModel(string group, IList<DayViewModel> days) {
        Group = group;
        Days = days;
    }
}