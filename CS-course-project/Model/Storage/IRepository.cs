using System.Threading.Tasks;

namespace CS_course_project.model.Storage; 

public interface IRepository<in T, TS, in TR> {
    void Update(T newItem);
    Task<TS> GetData();
    void RemoveAt(TR key);
}