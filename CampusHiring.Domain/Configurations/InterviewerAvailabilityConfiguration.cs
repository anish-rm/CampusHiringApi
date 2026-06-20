using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusHiring.Api.Domain.Configurations;

public class InterviewerAvailabilityConfiguration : IEntityTypeConfiguration<InterviewerAvailability>
{
    public void Configure(EntityTypeBuilder<InterviewerAvailability> builder)
    {
        builder.HasOne(ia => ia.Interviewer)
           .WithMany(i => i.InterviewerAvailabilities)
           .HasForeignKey(ia => ia.InterviewerUserId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}