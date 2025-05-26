namespace TeamTaskManagement.DTOs;

public class CreateTeamDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class TeamDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class AddUserToTeamDto
{
    public int UserId { get; set; }
}