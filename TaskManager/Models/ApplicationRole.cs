using Microsoft.AspNetCore.Identity;

namespace TaskManager.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        // 擴展角色屬性
        public string Description { get; set; } = string.Empty;

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
