using AutoMapper;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.ViewModels;

namespace TaskManager
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<EditTaskViewModel, TaskDTO>().ReverseMap();

            CreateMap<CreateTaskViewModel, TaskDTO>().ReverseMap();

            CreateMap<TaskDTO, DeleteTaskViewModel>().ReverseMap();


            CreateMap<TaskViewModel, TaskDTO>().ReverseMap();

            //for read
            CreateMap<TaskItem, TaskDTO>().ReverseMap();

            //for Update
            CreateMap<TaskDTO, TaskItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore());


            CreateMap<TaskItem, Models.Task>().ReverseMap();
        }
    }
}
