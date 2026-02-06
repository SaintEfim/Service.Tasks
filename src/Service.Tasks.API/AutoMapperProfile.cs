using AutoMapper;
using Service.Tasks.API.Models.Base;
using Service.Tasks.API.Models.Task;
using Service.Tasks.API.Models.User;
using Service.Tasks.Domain.Models.Task;
using Service.Tasks.Domain.Models.User;

namespace Service.Tasks.API;

public sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        MapTask();
        MapUser();
    }

    private void MapTask()
    {
        CreateMap<TaskModel, TaskDto>()
            .ReverseMap();

        CreateMap<TaskCreateDto, TaskModel>();

        CreateMap<TaskModel, TaskUpdateDto>()
            .ForMember(dest => dest.ChildIds, opt => opt.MapFrom(src => src.Children
                .Select(model => model.Id)
                .ToList()))
            .ReverseMap()
            .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.ChildIds
                .Select(id => new TaskModel { Id = id })
                .ToList()));

        CreateMap<TaskModel, CreateActionResultDto>();
    }

    private void MapUser()
    {
        CreateMap<UserModel, UserDto>();

        CreateMap<UserLoginDto, UserModel>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.ToLower()));

        CreateMap<UserRegisterDto, UserModel>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.ToLower()));

        CreateMap<AuthenticationModel, AuthenticationDto>();
    }
}
