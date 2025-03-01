using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllTasksAsync();
    Task<List<TaskItem>> GetTasksByUserIdAsync(Guid userId);
    Task<TaskItem> GetTaskByIdAsync(int id);
    Task AddTaskAsync(TaskItem task);
    Task<bool> UpdateTaskAsync(TaskItem task);
    Task DeleteTaskAsync(int id);
}
