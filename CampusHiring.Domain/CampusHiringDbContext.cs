using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CampusHiring.Api.Domain;

public class CampusHiringDbContext : IdentityDbContext<User>
{
   
    public CampusHiringDbContext(DbContextOptions<CampusHiringDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<InterviewerAvailability> InterviewerAvailabilities { get; set; }
    public DbSet<InterviewRound> InterviewRounds { get; set; }
    public DbSet<Interviewer> Interviewers { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<College> Colleges { get; set; }
    public DbSet<CandidateSelection> CandidateSelections { get; set; }
    public DbSet<AssessmentType> AssessmentTypes { get; set; }
    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<CollegeAdmin> CollegeAdmins { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
