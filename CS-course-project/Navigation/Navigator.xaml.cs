using System.Windows.Input;
using CS_course_project.model.Storage;
using CS_course_project.view;
using CS_course_project.View;
using CS_course_project.ViewModel;

namespace CS_course_project.Navigation; 

public partial class Navigator {
    public static RoutedCommand Navigate { get; } = new("RenderPage", typeof(Navigator));

    public Navigator() {
        InitializeComponent();
        Frame.Content = new Login();
        CommandBindings.Add(new CommandBinding(Navigate, RenderPage, (_, e) => e.CanExecute = true));      
    }

    private void RenderPage(object sender, ExecutedRoutedEventArgs e) {
        if (e.Parameter is not string) return;
        
        Frame.Content = e.Parameter switch {
            "Login" => new Login(),
            "AdminPanel" => new AdminPanel(),
            "Settings" => new Settings(),
            "Timetable" => new Timetable(),
            _ => Frame.Content
        };
    }
}