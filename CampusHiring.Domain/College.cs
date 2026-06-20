namespace CampusHiring.Api.Domain;

public class College : BasicInfo
{
    public int NirfRank { get; set; }
    public IList<Student> Students { get; set; } = [];
}
