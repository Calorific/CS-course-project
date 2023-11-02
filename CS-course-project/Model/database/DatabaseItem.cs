using System;

namespace CS_course_project.model.database; 

public abstract class DatabaseItem {
    public string GetUid() {
        throw new NotImplementedException();
    }
    
     protected static string GenerateUid() {
         new Settings(45, 10, 15, 480);
         return Guid.NewGuid().ToString();
     }
}