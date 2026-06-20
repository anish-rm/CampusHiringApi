using AutoMapper;
using CampusHiring.Api.Application.DTOs;
using CampusHiring.Api.Domain;

namespace CampusHiring.Api.Application.MappingProfiles;

public class AssessmentMappingProfile : Profile
{
    public AssessmentMappingProfile()
    {
        CreateMap<Assessment, GetAssessmentsDto>()
            .ForMember(d => d.StudentName, cfg => cfg.MapFrom(s => s.Student != null ? s.Student.User.UserName : string.Empty))
            .ForMember(d => d.CompanyName, cfg => cfg.MapFrom(s => s.Company != null ? s.Company.Name : string.Empty))
            .ForMember(d => d.AssessmentType, cfg => cfg.MapFrom(s => s.AssessmentType != null ? s.AssessmentType.Name : string.Empty));

        CreateMap<Assessment, GetAssessmentDto>()
            .ForMember(d => d.StudentName, cfg => cfg.MapFrom(s => s.Student != null ? s.Student.User.UserName : string.Empty))
            .ForMember(d => d.Cgpa, cfg => cfg.MapFrom(s => s.Student != null ? s.Student.Cgpa : 0))
            .ForMember(d => d.Department, cfg => cfg.MapFrom(s => s.Student.Department))
            .ForMember(d => d.CollegeName, cfg => cfg.MapFrom(s => s.Student != null ? s.Student.College.Name : string.Empty))
            .ForMember(d => d.CompanyName, cfg => cfg.MapFrom(s => s.Company != null ? s.Company.Name : string.Empty))
            .ForMember(d => d.AssessmentType, cfg => cfg.MapFrom(s => s.AssessmentType != null ? s.AssessmentType.Name : string.Empty))
            .ForMember(d => d.MaxScore, cfg => cfg.MapFrom(s => s.AssessmentType != null ? s.AssessmentType.MaxScore : 100))
            .ForMember(d => d.PassScore, cfg => cfg.MapFrom(s => s.AssessmentType != null ? s.AssessmentType.PassScore : 70));

        CreateMap<UpdateAssessmentDto, Assessment>();
        CreateMap<CreateAssessmentDto, Assessment>();
    }
}
