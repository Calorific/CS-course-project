using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CS_course_project.ViewModel; 

public class BaseViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void Notify([CallerMemberName] string prop = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
    
    protected void NotifyAll(params string[] props) {
        foreach (var prop in props) 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}