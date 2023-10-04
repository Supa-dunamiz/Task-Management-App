using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TaskApp.Models
{
    public class AppUser : IdentityUser
    {
        [Key]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ProfileImageUrl { get; set; }
        public ICollection<UserTask> UserTasks { get; set; }
    }
}
