using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskApp.Data;
using TaskApp.Interfaces;
using TaskApp.Models;

namespace TaskApp.Repository
{
    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserTaskRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Add(UserTask userTask)
        {
            _context.Add(userTask);
            return Save();
        }

        public bool Delete(UserTask userTask)
        {
            _context.Remove(userTask);
            return Save();
        }

        public async Task<IEnumerable<UserTask>> GetAll()
        {
            return await _context.UserTasks.ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetByDateAsync(DateTime date)
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            return await _context.UserTasks.Where(d => d.DateOfTask == date.Date)
                .Where(i => i.AppUserId == curUserId).ToListAsync();
        }

        public async Task<UserTask> GetByIdAsync(int id)
        {
            return await _context.UserTasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<UserTask> GetByIdNoTrackingAsync(int id)
        {
            return await _context.UserTasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<UserTask>> GetByPresentDateAsync()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            DateTime curDate = DateTime.Today;
            return await _context.UserTasks.Where(d => d.DateOfTask == curDate).Where(u => u.AppUserId == curUserId).ToListAsync();
        }

        public async Task<IEnumerable<UserTask>> GetByTitleAsync(string title)
        {
            return await _context.UserTasks.Where(t => t.Title == title).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(UserTask userTask)
        {
            _context.Update(userTask);
            return Save();
        }
    }
}
