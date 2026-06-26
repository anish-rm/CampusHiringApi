namespace CampusHiring.Api.Domain;

public class CollegeAdmin
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public User? User { get; set; }
    public required int CollegeId { get; set; }
    public College? College { get; set; }
}
