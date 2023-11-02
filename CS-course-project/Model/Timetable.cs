using CS_course_project.model.database;

namespace CS_course_project.model;

public class Timetable : DatabaseItem {
    private string _id = "";
    
    public new virtual string GetUid() {
        if (string.IsNullOrEmpty(_id))
            _id = GenerateUid();
        return "Timetable_ID-" + _id;
    }

    
}