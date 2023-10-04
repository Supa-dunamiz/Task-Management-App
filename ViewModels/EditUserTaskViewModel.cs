using TaskApp.Data.Enum;

namespace TaskApp.ViewModels
{
    public class EditUserTaskViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DateOfTask { get; set; }
        public string AppUserId { get; set; }
        public TaskUpdate? TaskUpdate { get; set; }
        public TaskPriority TaskPriority { get; set; }
    }
}
