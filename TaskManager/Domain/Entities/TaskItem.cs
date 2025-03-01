using System.Net.NetworkInformation;
using TaskManager.Models;

namespace TaskManager.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser? User { get; set; }  

    }
}
