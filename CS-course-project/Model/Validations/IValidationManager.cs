using System.Threading.Tasks;
using CS_course_project.Model.Storage;
using CS_course_project.Model.Timetables;

namespace CS_course_project.Model.Validations; 

public interface IValidationManager { 
    Task ValidateNewTimetable(IDataManager dataManager, ITimetable newTimetable);
    
    
    Task ValidateNewGroup(IDataManager dataManager, string newGroup);
    
    
    Task ValidateNewTeacher(IDataManager dataManager, ITeacher newTeacher);
    
    Task ValidateRemovingTeacher(IDataManager dataManager, string id);


    Task ValidateNewClassroom(IDataManager dataManager, string newClassroom);
    
    Task ValidateRemovingClassroom(IDataManager dataManager, string item);


    Task ValidateNewSubject(IDataManager dataManager, string newSubject);

    Task ValidateRemovingSubject(IDataManager dataManager, string item);
}