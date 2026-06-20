using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusHiring.Api.Domain.Configurations;

public class CandidateSelectionConfiguration : IEntityTypeConfiguration<CandidateSelection>
{
    public void Configure(EntityTypeBuilder<CandidateSelection> builder)
    {
        builder.Property(cs => cs.CandidateStatus)
            .HasConversion<string>()
            .HasMaxLength(20);
    }

}
