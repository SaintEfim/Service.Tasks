using AutoMapper;
using Service.Tasks.Domain.Models;
using Service.Tasks.Data.Models;

namespace Service.Tasks.Domain;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TaskEntity, TaskModel>()
            .ReverseMap()
            .ForMember(dest => dest.Children, opt => opt.Ignore())
            .ForMember(dest => dest.Parent, opt => opt.Ignore());
    }
}
