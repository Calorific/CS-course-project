using CS_course_project.Model.Services.AuthServices;
using CS_course_project.Model.Services.TimeServices;
using CS_course_project.Model.Storage;
using CS_course_project.Model.Validations;

namespace CS_course_project;

public partial class App {
    public App() {
        InitializeComponent();

        var validationManager = new ValidationManager();
        var groupsRepository = new BaseRepository(RepositoryItems.Groups);
        var classroomsRepository = new BaseRepository(RepositoryItems.Classrooms);
        var subjectsRepository = new BaseRepository(RepositoryItems.Subjects);
        var teachersRepository = new TeachersRepository();
        var timetablesRepository = new TimetablesRepository();
        var settingsRepository = new SettingsRepository();
        var sessionRepository = new SessionRepository();
        
        IDataManager dataManager = new DataManager(validationManager, groupsRepository, classroomsRepository, subjectsRepository, teachersRepository,
            timetablesRepository, settingsRepository, sessionRepository);

        IAuthService authService = new AuthService(dataManager);

        ITimeConverter timeConverter = new TimeConverter();

        Current.Dispatcher.Invoke(() => {
            MainWindow = new MainWindow(dataManager, authService, timeConverter);
            MainWindow.Show();
        });
    }
}