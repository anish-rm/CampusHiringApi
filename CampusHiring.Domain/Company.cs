namespace CampusHiring.Api.Domain;

public class Company : BasicInfo
{
    public string Type { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public IList<Interviewer> Interviewers { get; set; } = [];
}
