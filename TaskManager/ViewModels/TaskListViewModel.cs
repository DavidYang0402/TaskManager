namespace TaskManager.ViewModels
{
    public class TaskListViewModel
    {
        public List<TaskViewModel> Tasks { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}
