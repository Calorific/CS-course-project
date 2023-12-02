using System.Collections.Generic;

namespace CS_course_project.Model.Timetables; 

public class Day : IDay {
    private readonly int _index;
    private readonly string[] _names = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

    public string Name => _names[_index];
    public IList<ILesson>? Lessons { get; set; }
    
    public Day(int index, IList<ILesson>? lessons) {
        _index = index;
        if (lessons != null)
            Lessons = lessons;
    }
}