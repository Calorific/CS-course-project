using System;
using System.Windows.Input;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Services.TimeServices;
using CS_course_project.model.Storage;
using CS_course_project.UserControls.AdminPanelPage;
using CS_course_project.UserControls.SettingsPage;
using CS_course_project.view;
using CS_course_project.View;
using CS_course_project.ViewModel;
using CS_course_project.ViewModel.UserControls;
using CS_course_project.ViewModel.UserControls.TimetableForm;

namespace CS_course_project.Navigation;

public interface INavigator {
    public RoutedCommand Navigate { get; }
}

public enum Pages {
    Login, AdminPanel, Settings, Timetable
}

public partial class Navigator : INavigator {
    public RoutedCommand Navigate { get; } = new("RenderPage", typeof(Navigator));

    private readonly IDataManager _dataManager;
    private readonly IAuthService _authService;
    private readonly ITimeConverter _timeConverter;

    public Navigator() {
        InitializeComponent();
        CommandBindings.Add(new CommandBinding(Navigate, RenderPage, (_, e) => e.CanExecute = true));    
        
        var groupsRepository = new BaseRepository(RepositoryItems.Groups);
        var classroomsRepository = new BaseRepository(RepositoryItems.Classrooms);
        var subjectsRepository = new BaseRepository(RepositoryItems.Subjects);
        var teachersRepository = new TeachersRepository();
        var timetablesRepository = new TimetablesRepository();
        var settingsRepository = new SettingsRepository();
        var sessionRepository = new SessionRepository();
        _dataManager = new DataManager(groupsRepository, classroomsRepository, subjectsRepository, teachersRepository,
            timetablesRepository, settingsRepository, sessionRepository);

        _authService = new AuthService(_dataManager);

        _timeConverter = new TimeConverter();
        
        Frame.Content = new Login {
            DataContext = new LoginViewModel(this, _dataManager, _authService)
        };
    }

    private void RenderPage(object sender, ExecutedRoutedEventArgs e) {
        if (e.Parameter is not Pages pageName) return;

        switch (pageName) {
            case Pages.Login: {
                Frame.Content = new Login {
                    DataContext = new LoginViewModel(this, _dataManager, _authService)
                };
                break;
            }
            case Pages.AdminPanel: {
                var children = new[] {
                    new TimetableForm { DataContext = new TimetableFormViewModel(this, _dataManager) } 
                };
                var page = new AdminPanel(children) {
                    DataContext = new AdminPanelViewModel(this, _authService)
                };
                Frame.Content = page;
                break;
            }
            case Pages.Settings: {
                var children = new[] {
                    new SettingsForm { DataContext = new SettingsFormViewModel(_dataManager, _timeConverter) }
                };
                var page = new Settings(children) {
                    DataContext = new SettingsViewModel(this, _dataManager)
                };
                Frame.Content = page;
                break;
            }
            case Pages.Timetable: {
                var page = new Timetable {
                    DataContext = new TimetablePageViewModel(this, _dataManager, _authService, _timeConverter)
                };
                Frame.Content = page;
                break;
            }
            default:
                throw new ArgumentException("Некорректное значение страницы");
        }
    }
}