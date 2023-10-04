using TaskApp.Models;

namespace TaskApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<UserTask>> GetUserTasks();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetUserByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
