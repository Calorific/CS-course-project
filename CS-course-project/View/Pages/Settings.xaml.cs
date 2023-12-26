using System.Collections.Generic;
using System.Windows.Controls;

namespace CS_course_project.View.Pages;

public partial class Settings {
    public Settings(IReadOnlyList<UserControl> children) {
        InitializeComponent();
        SettingsForm.Content = children[0];
    }
}