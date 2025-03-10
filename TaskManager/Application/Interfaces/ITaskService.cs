using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.ViewModels;

public interface ITaskService
{
    Task<List<TaskDTO>> GetAllTasksAsync();
    Task<List<TaskDTO>> GetTasksByUserIdAsync(Guid userId);
    Task<TaskDTO> GetTaskByIdAsync(int id);
    Task CreateTaskAsync(TaskDTO task);
    Task<bool> UpdateTaskAsync(TaskDTO task);
    Task DeleteTaskAsync(int id);
}
