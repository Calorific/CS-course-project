using CS_course_project.ViewModel.Components.TimetableForm;

namespace CS_course_project.Tests.ViewModel.Components.TimetableForm; 

public class TimetableFormViewModelTests {
    [Fact]
    public void ShouldAddErrorWhenTimetablesOverlapClassroom() {
        // Assert
        var viewModel = new TimetableFormViewModel(new MockNavigator(), new MockDataManager()) {
            // Act
            CurrentGroup = "group2"
        };

        Task.Run(() => {
            // Act
            viewModel.CurrentTimetable.Days[0].Lessons[0].Classroom = "classroom";
            viewModel.CurrentTimetable.Days[0].Lessons[0].Subject = "unique";
            viewModel.CurrentTimetable.Days[0].Lessons[0].Teacher = "unique";


            // Assert
            Assert.Single(viewModel.CurrentTimetable.Days[0].Lessons[0].Errors);
        });
    }
    
    [Fact]
    public void ShouldAddErrorWhenTimetablesOverlapTeacher() {
        // Assert
        var viewModel = new TimetableFormViewModel(new MockNavigator(), new MockDataManager()) {
            // Act
            CurrentGroup = "group2"
        };

        Task.Run(() => {
            // Act
            viewModel.CurrentTimetable.Days[0].Lessons[0].Classroom = "unique";
            viewModel.CurrentTimetable.Days[0].Lessons[0].Subject = "unique";
            viewModel.CurrentTimetable.Days[0].Lessons[0].Teacher = "teacher";


            // Assert
            Assert.Single(viewModel.CurrentTimetable.Days[0].Lessons[0].Errors);
        });
    }
}