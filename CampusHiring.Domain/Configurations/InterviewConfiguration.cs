using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusHiring.Api.Domain.Configurations;

public class InterviewConfiguration : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        builder.Property(i => i.InterviewStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(i => i.InterviewRound)
           .WithMany(ir => ir.Interviews)
           .HasForeignKey(i => i.InterviewRoundId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Interviewer)
           .WithMany(ir => ir.Interviews)
           .HasForeignKey(i => i.InterviewerUserId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
