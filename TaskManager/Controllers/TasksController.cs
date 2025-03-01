using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.ViewModels;

[AllowAnonymous]
public class TasksController : Controller
{
    private readonly ITaskService _taskService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public TasksController(ITaskService taskService, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _taskService = taskService;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            var allTasks = await _taskService.GetAllTasksAsync();
            var allTasksViewModel = _mapper.Map<List<TaskViewModel>>(allTasks);
            return View(new TaskListViewModel { Tasks = allTasksViewModel});
        }

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!Guid.TryParse(userIdString, out var currentUserId)) return Unauthorized();

        var tasks = await _taskService.GetTasksByUserIdAsync(currentUserId);
        var taskViewModels = _mapper.Map<List<TaskViewModel>>(tasks);

        return View(new TaskListViewModel 
        {
            Tasks = taskViewModels,
            CurrentUserId = currentUserId
        });
    }

    public async Task<IActionResult> Details(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        var taskViewModel = _mapper.Map<TaskViewModel>(task);
        return View(taskViewModel);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateTaskViewModel model)
    {
        if(!ModelState.IsValid) return View(model);

        if(!User.Identity.IsAuthenticated) return Unauthorized();

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!Guid.TryParse(userIdString, out var currentUserId)) return Unauthorized();

        var task = _mapper.Map<TaskDTO>(model);
        task.UserId = currentUserId;

        await _taskService.CreateTaskAsync(task);
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var taskDto = await _taskService.GetTaskByIdAsync(id);
        if (taskDto == null)
        {
            return NotFound();
        }

        if(!User.Identity.IsAuthenticated) return Unauthorized();

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var currentUserId)) return Unauthorized();

        // 檢查該任務是否屬於當前用戶
        if (taskDto.UserId != currentUserId)
        {
            return Unauthorized();  // 如果用戶沒有權限更改這個任務
        }

        // 將 DTO 轉換為 ViewModel，方便於前端顯示
        var editTaskViewModel = _mapper.Map<EditTaskViewModel>(taskDto);

        return View(editTaskViewModel);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditTaskViewModel model, int id)
    {
        if (!ModelState.IsValid) return View(model);

        if (!User.Identity.IsAuthenticated) return Unauthorized();

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var currentUserId)) return Unauthorized();

        var taskDto = await _taskService.GetTaskByIdAsync(id);
        if (taskDto == null) return NotFound();

        if (taskDto.UserId != currentUserId) return Forbid();

        var updateTask = _mapper.Map<TaskDTO>(model);
        updateTask.Id = id;
        updateTask.UserId = currentUserId;
        updateTask.CreateAt = taskDto.CreateAt;

        var result = await _taskService.UpdateTaskAsync(updateTask);
        if(!result) return BadRequest();

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        if (!User.Identity.IsAuthenticated) return Unauthorized();

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var currentUserId)) return Unauthorized();

        if (task.UserId != currentUserId) return Forbid();

        var deleteTaskViewModel = _mapper.Map<DeleteTaskViewModel>(task);

        return View(deleteTaskViewModel);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize]
    public async Task<IActionResult> DeletePost(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        if (!User.Identity.IsAuthenticated) return Unauthorized();

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        if (task.UserId != userId) return Forbid();

        await _taskService.DeleteTaskAsync(task.Id);
        return RedirectToAction(nameof(Index));
    }
}
