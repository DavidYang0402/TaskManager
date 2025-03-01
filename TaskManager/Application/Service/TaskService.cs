using AutoMapper;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.ViewModels;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<List<TaskDTO>> GetAllTasksAsync()
    {
        var tasks = await _taskRepository.GetAllTasksAsync();
        return _mapper.Map<List<TaskDTO>>(tasks);
    }

    public async Task<List<TaskDTO>> GetTasksByUserIdAsync(Guid userId)
    {
        var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
        return _mapper.Map<List<TaskDTO>>(tasks);
    }

    public async Task<TaskDTO> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        if (task == null)
        {
            return null;
        }

        return new TaskDTO
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            UserId = task.UserId
        };
    }

    public async Task CreateTaskAsync(TaskDTO taskDto)
    {

        var task = new TaskItem
        {
            Title = taskDto.Title,
            Description = taskDto.Description,
            Status = taskDto.Status,
            UserId = taskDto.UserId,
            CreateAt = DateTime.Now,
        };

        await _taskRepository.AddTaskAsync(task);
    }

    public async Task<bool> UpdateTaskAsync(TaskDTO taskDto)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskDto.Id);
        if (task == null)
        {
            return false; // 任務不存在或用戶無權限更新
        }

        _mapper.Map(taskDto, task);

        await _taskRepository.UpdateTaskAsync(task);
        return true;
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);

        if (task != null)
        {
            await _taskRepository.DeleteTaskAsync(id);
        }

        await _taskRepository.DeleteTaskAsync(id);

    }
}
