using System.Collections.Generic;

namespace CS_course_project.ViewModel.Components.TimetableForm; 

public class TimetableViewModel {
    public IList<DayViewModel> Days { get; }

    public TimetableViewModel(IList<DayViewModel> days) {
        Days = days;
    }
}