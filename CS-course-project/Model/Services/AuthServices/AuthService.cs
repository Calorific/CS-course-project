using System.Linq;
using System.Threading.Tasks;
using CS_course_project.Model.Storage;

namespace CS_course_project.Model.Services.AuthServices; 

public class AuthService : IAuthService {
    private readonly IDataManager _dataManager;

    public AuthService(IDataManager dataManager) {
        _dataManager = dataManager;
    }
    
    private async Task<string?> LogInAdmin(string password) {
        var hashedAdminPassword = (await _dataManager.GetSettings()).HashedAdminPassword;
        var isValid = BCrypt.Net.BCrypt.Verify(password, hashedAdminPassword);
        
        if (!isValid) {
            await _dataManager.RemoveSession();
            return "WRONG_PASSWORD";
        }
        await _dataManager.UpdateSession(new Session(true, password));
        return null;
    }
    
    private async Task<string?> LogInUser(string group) {
        var groups = (await _dataManager.GetTimetables()).Keys;
        if (!groups.Contains(group))
            return "INVALID_GROUP";
        await _dataManager.UpdateSession(new Session(false, group));
        return null;
    }
    
    public async Task<string?> LogIn(string data, bool isAdmin) {
        return isAdmin ? await LogInAdmin(data) : await LogInUser(data);
    }

    public async Task<bool> LogOut() {
        return await _dataManager.RemoveSession();
    }
}

