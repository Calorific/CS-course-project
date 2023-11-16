using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CS_course_project.ViewModel.UserControls;

namespace CS_course_project.UserControls.SettingsPage; 

public partial class EditableList {
    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<string>), typeof(EditableList));

    public ObservableCollection<string> Items {
        get => (ObservableCollection<string>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }
    
    public static readonly DependencyProperty AddCommandProperty =
        DependencyProperty.Register(nameof(AddCommand), typeof(ICommand), typeof(EditableList));

    public ICommand AddCommand {
        get => (ICommand)GetValue(AddCommandProperty);
        set {
            SetValue(AddCommandProperty, value);
            Form.DataContext = new EditableListViewModel(Items, value);
        }
    }
    
    public static readonly DependencyProperty RemoveCommandProperty =
        DependencyProperty.Register(nameof(RemoveCommand), typeof(ICommand), typeof(EditableList));

    public ICommand RemoveCommand {
        get => (ICommand)GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }
    
    private void OnLoad(object sender, RoutedEventArgs e) {
        if (Form.DataContext is not EditableListViewModel viewModel) return;
        viewModel.BaseCommand = AddCommand;
        viewModel.Items = Items;
    }
    
    public EditableList() {
        InitializeComponent();
        ItemListBox.DataContext = this;
        Loaded += OnLoad;
    }
}