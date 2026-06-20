namespace CampusHiring.Api.Domain;

public class Company : BasicInfo
{
    public string? Type { get; set; }
    public string? Website { get; set; }
    public IList<Interviewer> Interviewers { get; set; } = [];
}
