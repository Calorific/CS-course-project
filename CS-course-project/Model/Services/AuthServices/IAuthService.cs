using System.Threading.Tasks;

namespace CS_course_project.Model.Services.AuthServices;

public interface IAuthService {
    public Task<string?> LogIn(string data, bool isAdmin);

    public Task<bool> LogOut();
}