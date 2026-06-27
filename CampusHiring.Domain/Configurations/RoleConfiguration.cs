using CampusHiring.Api.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusHiring.Api.Domain.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "58da3b22-a0f4-4f53-8c98-bb4aabd69b60",
                Name = RoleNames.Admin,
                NormalizedName = RoleNames.Admin.ToUpper(),
                ConcurrencyStamp = "762167d2-790c-4392-a64a-f641e54a1f0e"
            },
            new IdentityRole
            {
                Id = "67b715fe-d9bf-4f50-b0c0-37599ba4fb6b",
                Name = RoleNames.Student,
                NormalizedName = RoleNames.Student.ToUpper(),
                ConcurrencyStamp = "1587469f-1e01-4c94-9e72-32ec1aa0018d"
            },
            new IdentityRole
            {
                Id = "964ab596-c9ba-4397-8378-42d8bc792af2",
                Name = RoleNames.Interviewer,
                NormalizedName = RoleNames.Interviewer.ToUpper(),
                ConcurrencyStamp = "82833d51-a7ba-4c5f-bb44-a2e923c3e6b3"
            },
            new IdentityRole
            {
                Id = "f90eaaa0-013e-4b3b-90c3-79f155289704",
                Name = RoleNames.CollegeAdmin,
                NormalizedName = RoleNames.CollegeAdmin.ToUpper(),
                ConcurrencyStamp = "33595ae8-4367-4d22-bf3b-a2f3a06583c6"
            }
        );
    }
}
