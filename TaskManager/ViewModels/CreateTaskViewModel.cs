namespace TaskManager.ViewModels
{
    public class CreateTaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "待辦";
        public Guid UserId { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}
