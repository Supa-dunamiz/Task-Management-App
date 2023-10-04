using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskApp.Data.Enum;

namespace TaskApp.Models
{
    public class UserTask
    {
        [Key] 
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DateOfTask { get; set; }
        public TaskPriority TaskPriority { get; set; }
        public TaskUpdate? TaskUpdate { get; set; }  
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}

