using System.Linq;
using System.Threading.Tasks;
using CS_course_project.model.Storage;

namespace CS_course_project.Model.Services.AuthServices; 

public static class AuthService {

    private static async Task<string?> LogInAdmin(string password) {
        var hashedAdminPassword = (await DataManager.LoadSettings()).HashedAdminPassword;
        var isValid = BCrypt.Net.BCrypt.Verify(password, hashedAdminPassword);
        
        if (!isValid) {
            await DataManager.RemoveSession();
            return "WRONG_PASSWORD";
        }
        await DataManager.UpdateSession(new Session(true, password));
        return null;
    }
    
    private static async Task<string?> LogInUser(string group) {
        var groups = (await DataManager.LoadTimetables()).Keys;
        if (!groups.Contains(group))
            return "INVALID_GROUP";
        await DataManager.UpdateSession(new Session(false, group));
        return null;
    }
    
    public static async Task<string?> LogIn(string data, bool isAdmin) {
        return isAdmin ? await LogInAdmin(data) : await LogInUser(data);
    }

    public static async Task<bool> LogOut() {
        return await DataManager.RemoveSession();
    }
}

