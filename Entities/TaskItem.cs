namespace TeamTaskManagement.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public long AssignedToUserId { get; set; }
        public User AssignedTo { get; set; }
        public long CreatedByUserId { get; set; }
        public User CreatedBy { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
