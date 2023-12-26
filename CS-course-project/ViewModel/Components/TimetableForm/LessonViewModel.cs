using System;
using CS_course_project.ViewModel.Common;

namespace CS_course_project.ViewModel.Components.TimetableForm;

public class LessonViewModel : NotifyErrorsViewModel {
    private readonly int _dayIdx;
    private readonly int _lessonIdx;

    private readonly Func<int, int, string?> _validateSubject;
    private readonly Func<int, int, string?> _validateClassroom;
    private readonly Func<int, int, string?> _validateTeacher;

    private string _subject;
    public string Subject {
        get => _subject;
        set {
            _subject = value;
            ClearErrors(nameof(Subject));
            if (string.IsNullOrEmpty(_subject)) {
                if (string.IsNullOrEmpty(_teacher) && string.IsNullOrEmpty(_classroom)) {
                    ClearErrors(nameof(Classroom));
                    ClearErrors(nameof(Teacher));
                }
                else {
                    AddError(nameof(Subject), "Нужно указать предмет");
                }
            }
            else {
                if (string.IsNullOrEmpty(_classroom))
                    AddError(nameof(Classroom), "Нужно указать аудиторию");
                if (string.IsNullOrEmpty(_teacher))
                    AddError(nameof(Teacher), "Нужно указать преподователя");
                var error = _validateSubject(_dayIdx, _lessonIdx);
                if (error != null)
                    AddError(nameof(Subject), error);
            }

            Notify();
        }
    }

    private string _classroom;
    public string Classroom {
        get => _classroom;
        set {
            _classroom = value;
            ClearErrors(nameof(Classroom));
            
            if (string.IsNullOrEmpty(_classroom)) {
                if (string.IsNullOrEmpty(_teacher) && string.IsNullOrEmpty(_subject)) {
                    ClearErrors(nameof(Subject));
                    ClearErrors(nameof(Teacher));
                }
                else {
                    AddError(nameof(Classroom), "Нужно указать аудиторию");
                }
            }
            else {
                if (string.IsNullOrEmpty(_subject))
                    AddError(nameof(Subject), "Нужно указать предмет");
                if (string.IsNullOrEmpty(_teacher))
                    AddError(nameof(Teacher), "Нужно указать преподователя");
                var error = _validateClassroom(_dayIdx, _lessonIdx);
                if (error != null)
                    AddError(nameof(Classroom), error);
            }

            Notify();
        }
    }

    private string _teacher;
    public string Teacher {
        get => _teacher;
        set {
            _teacher = value;
            ClearErrors(nameof(Teacher));
            if (string.IsNullOrEmpty(_teacher)) {
                if (string.IsNullOrEmpty(_subject) && string.IsNullOrEmpty(_classroom)) {
                    ClearErrors(nameof(Subject));
                    ClearErrors(nameof(Classroom));
                }
                else {
                    AddError(nameof(Teacher), "Нужно указать преподавателя");
                }
            }
            else {
                if (string.IsNullOrEmpty(_subject))
                    AddError(nameof(Subject), "Нужно указать предмет");
                if (string.IsNullOrEmpty(_classroom))
                    AddError(nameof(Classroom), "Нужно указать аудиторию");
                var error = _validateTeacher(_dayIdx, _lessonIdx);
                if (error != null)
                    AddError(nameof(Teacher), error);
            }
            Notify();
        }
    }

public LessonViewModel(int dayIdx, int lessonIdx, Func<int, int, string?> validateSubject, Func<int, int, string?> validateClassroom, Func<int, int, string?> validateTeacher) {
        _dayIdx = dayIdx;
        _lessonIdx = lessonIdx;

        _validateSubject = validateSubject;
        _validateClassroom = validateClassroom;
        _validateTeacher = validateTeacher;
        
        _subject = string.Empty;
        _classroom = string.Empty;
        _teacher = string.Empty;
    }
}