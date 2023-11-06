namespace CS_course_project.model.Storage; 

public interface IRepository<in T, out TS, in TR> {
    void Update(T newItem);
    TS GetData();
    void RemoveAt(TR key);
}