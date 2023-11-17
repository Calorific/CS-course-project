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

        var adminPasswordHash = (await DataManager.LoadSettings()).HashedAdminPassword;
        return BCrypt.Net.BCrypt.Verify(data, adminPasswordHash) ? null : "WRONG_PASSWORD";
    }
}

