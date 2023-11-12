using System.Collections.Generic;
using System.Linq;

namespace CS_course_project.Model.Timetables; 

public class Day {
    private readonly int _index;
    private readonly string[] _names = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

    public string Name => _names[_index];
    public List<Lesson> Lessons = Enumerable.Range(0, 25).Select(_ => new Lesson()).ToList();
    
    public Day(int index, List<Lesson>? lessons) {
        _index = index;
        if (lessons != null)
            Lessons = lessons;
    }
}