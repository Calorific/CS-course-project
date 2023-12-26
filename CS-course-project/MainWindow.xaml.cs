using System;
using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Services.TimeServices;
using CS_course_project.Model.Storage;
using CS_course_project.View.Components.AdminPanel;
using CS_course_project.View.Components.Settings;
using CS_course_project.View.Pages;
using CS_course_project.ViewModel.Components.Settings;
using CS_course_project.ViewModel.Components.TimetableForm;
using CS_course_project.ViewModel.Pages;

namespace CS_course_project; 

public interface INavigator {
    void Navigate(Routes pageName);
}

public enum Routes {
    Login, AdminPanel, Settings, Timetable
}

public partial class MainWindow : INavigator {
    private readonly IDataManager _dataManager;
    private readonly IAuthService _authService;
    private readonly ITimeConverter _timeConverter;

    public MainWindow(IDataManager dataManager, IAuthService authService, ITimeConverter timeConverter) {
        InitializeComponent();

        _dataManager = dataManager;
        _authService = authService;
        _timeConverter = timeConverter;
        
        Navigate(Routes.Login);
    }

    public void Navigate(Routes pageName) {
        switch (pageName) {
            case Routes.Login: {
                Frame.Content = new Login {
                    DataContext = new LoginViewModel(this, _dataManager, _authService)
                };
                break;
            }
            case Routes.AdminPanel: {
                var children = new[] {
                    new TimetableForm { DataContext = new TimetableFormViewModel(this, _dataManager) } 
                };
                var page = new AdminPanel(children) {
                    DataContext = new AdminPanelViewModel(this, _authService)
                };
                Frame.Content = page;
                break;
            }
            case Routes.Settings: {
                var children = new[] {
                    new SettingsForm { DataContext = new SettingsFormViewModel(_dataManager, _timeConverter) }
                };
                var page = new Settings(children) {
                    DataContext = new SettingsViewModel(this, _dataManager)
                };
                Frame.Content = page;
                break;
            }
            case Routes.Timetable: {
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