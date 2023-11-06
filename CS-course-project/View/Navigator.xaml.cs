using System.Windows.Input;
using CS_course_project.view;

namespace CS_course_project.View; 

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
            "AdminPanel" => new AdminPanel(),
            _ => Frame.Content
        };
    }
}