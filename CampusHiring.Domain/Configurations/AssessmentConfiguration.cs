using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CampusHiring.Api.Domain.Configurations;
//Add-Migration Initial -Project CampusHiring.Api.Domain -StartupProject CampusHiring.Api
public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> builder)
    {
        builder.HasOne(a => a.AssessmentType)
           .WithMany(at => at.Assessments)
           .HasForeignKey(a => a.AssessmentTypeId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
