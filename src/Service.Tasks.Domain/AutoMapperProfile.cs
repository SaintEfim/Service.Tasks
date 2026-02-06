using AutoMapper;
using Service.Tasks.Domain.Models;
using Service.Tasks.Data.Models;
using Service.Tasks.Domain.Models.User;

namespace Service.Tasks.Domain;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TaskEntity, TaskModel>()
            .ReverseMap()
            .ForMember(dest => dest.Children, opt => opt.Ignore())
            .ForMember(dest => dest.Parent, opt => opt.Ignore());

        CreateMap<UserEntity, UserModel>()
            .ReverseMap();
    }
}
