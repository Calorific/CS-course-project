using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace CS_course_project.Model.Services; 

public class AuthService {
    public string? LogIn(string data, bool isAdmin) {
        if (!isAdmin) {
            MessageBox.Show(data);
            return null;
        }
        
        var hashedPassword = Convert.ToBase64String(MD5.HashData(Encoding.UTF8.GetBytes(data)));
        var isValid = hashedPassword == "ICy5YqxZB1uWSwcVLSNLcA==";
        return isValid ? null : "WRONG_PASSWORD";
    }
}

