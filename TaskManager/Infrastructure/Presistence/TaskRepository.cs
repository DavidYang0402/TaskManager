using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs;
using AutoMapper;

public class TaskRepository : ITaskRepository
{
    private readonly TasksContext _context;
    private readonly IMapper _mapper;

    public TaskRepository(TasksContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        var taskItems = await _context.Tasks.ToListAsync();
        return _mapper.Map<List<TaskItem>>(taskItems);
    }

    public async Task<List<TaskItem>> GetTasksByUserIdAsync(Guid userId)
    {
        var taskItems = await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        return _mapper.Map<List<TaskItem>>(taskItems);
    }

    public async Task<TaskItem> GetTaskByIdAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            throw new KeyNotFoundException($"Task with Id {id} not found.");
        }

        return _mapper.Map<TaskItem>(task);
    }

    public async System.Threading.Tasks.Task AddTaskAsync(TaskItem taskItem)
    {
        var task = _mapper.Map<TaskManager.Models.Task>(taskItem);

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateTaskAsync(TaskItem taskItem)
    {
        var task = await _context.Tasks.FindAsync(taskItem.Id);
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with Id {taskItem.Id} not found.");
        }

        //覆蓋原有的屬性
        _mapper.Map(taskItem, task);

        _context.Tasks.Update(task);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
