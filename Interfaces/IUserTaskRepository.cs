using TaskApp.Models;

namespace TaskApp.Interfaces
{
    public interface IUserTaskRepository
    {
        Task<IEnumerable<UserTask>> GetAll();
        Task<UserTask> GetByIdAsync(int id);
        Task<UserTask> GetByIdNoTrackingAsync(int id);
        Task<IEnumerable<UserTask>> GetByTitleAsync(string title);
        Task<IEnumerable<UserTask>> GetByDateAsync(DateTime date);
        Task<IEnumerable<UserTask>> GetByPresentDateAsync();
        bool Add(UserTask userTask);
        bool Update(UserTask userTask);
        bool Delete(UserTask userTask);
        bool Save();
    }
}
