namespace TeamTaskManagement.Models
{
    public class TeamUser
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        
    }
}
