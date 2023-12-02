using System.Threading.Tasks;
using System.Windows;
using CS_course_project.model.Storage;

namespace CS_course_project.Model.Services; 

public static class AuthService {
    public static async Task<string?> LogIn(string data, bool isAdmin) {
        if (!isAdmin) {
            MessageBox.Show(data);
            return null;
        }
        
        var hashedAdminPassword = (await DataManager.LoadSettings()).HashedAdminPassword;
        var isValid = BCrypt.Net.BCrypt.Verify(data, hashedAdminPassword);
        
        if (!isValid) return "WRONG_PASSWORD";
        await DataManager.UpdateSession(new Session(true, data));
        return null;
    }

    public static async Task<bool> LogOut() {
        return await DataManager.RemoveSession();
    }
}

