using System.Collections.Generic;
using System.Windows.Controls;

namespace CS_course_project.View.Pages; 

public partial class AdminPanel {
    public AdminPanel(IReadOnlyList<UserControl> children) {
        InitializeComponent();
        TimetableForm.Content = children[0];
    }
}