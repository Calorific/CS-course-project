using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CS_course_project.UserControls.SettingsPage; 

public partial class EditableList {
    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<string>), typeof(EditableList));

    public ObservableCollection<string> Items {
        get => (ObservableCollection<string>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }
    
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(RemoveCommand), typeof(ICommand), typeof(EditableList));

    public ICommand RemoveCommand {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    
    public EditableList() {
        InitializeComponent();
        TeachersListBox.DataContext = this;
    }
}