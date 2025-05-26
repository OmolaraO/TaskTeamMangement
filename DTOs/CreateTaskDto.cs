namespace TeamTaskManagement.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public int AssignedToUserId { get; set; }
}

public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public TeamTaskManagement.Models.TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public long AssignedToUserId { get; set; }
    public long CreatedByUserId { get; set; }
    public int TeamId { get; set; }
}

public class UpdateTaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public int AssignedToUserId { get; set; }
}

public class UpdateTaskStatusDto
{
    public TeamTaskManagement.Models.TaskStatus Status { get; set; }
}