namespace TaskManager.Application.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
