using System.Threading.Tasks;

namespace CS_course_project.Model.Storage; 

public interface IRepository<in T, TS, in TR> {
    Task<bool> Update(T newItem);
    Task<TS> Read();
    Task<bool> Delete(TR key);
}