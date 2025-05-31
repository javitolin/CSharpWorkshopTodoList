using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoWebApp.Entities;
using ToDoWebApp.IO.Interfaces;

namespace ToDoWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly ITaskManager _taskManager;

        public TasksController(ILogger<TasksController> logger, ITaskManager taskManager)
        {
            _logger = logger;
            _taskManager = taskManager;
        }

        [HttpGet]
        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(CancellationToken cancellationToken)
        {
            var tasks = await _taskManager.GetAllTasksAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} tasks", tasks.Count());
            return tasks;
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskItem>> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken)
        {
            var task = await _taskManager.GetTaskByIdAsync(taskId, cancellationToken);
            if (task == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found", taskId);
                return NotFound();
            }

            _logger.LogInformation("Retrieved task with ID {TaskId}", taskId);
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> AddTaskAsync([FromBody] TaskItem task, CancellationToken cancellationToken)
        {
            if (task == null)
            {
                _logger.LogError("Received null task for addition");
                return BadRequest("Task cannot be null");
            }

            if (await _taskManager.GetTaskByIdAsync(task.Id, cancellationToken) != null)
            {
                _logger.LogWarning("Task with ID {TaskId} already exists", task.Id);
                return Conflict("Task with the same ID already exists");
            }

            await _taskManager.AddTaskAsync(task, cancellationToken);
            _logger.LogInformation("Added new task with ID {TaskId}", task.Id);
            return Ok(task.Id);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTaskAsync(Guid taskId, [FromBody] TaskItem updatedTask, CancellationToken cancellationToken)
        {
            if (updatedTask == null)
            {
                _logger.LogError("Received null task for update");
                return BadRequest("Task cannot be null");
            }

            var success = await _taskManager.UpdateTaskAsync(taskId, updatedTask, cancellationToken);
            if (!success)
            {
                _logger.LogWarning("Failed to update task with ID {TaskId}", taskId);
                return NotFound();
            }

            _logger.LogInformation("Updated task with ID {TaskId}", taskId);
            return Ok(taskId);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken)
        {
            var success = await _taskManager.RemoveTaskAsync(taskId, cancellationToken);
            if (!success)
            {
                _logger.LogWarning("Failed to delete task with ID {TaskId}", taskId);
                return NotFound();
            }
            _logger.LogInformation("Deleted task with ID {TaskId}", taskId);
            return Ok(taskId);
        }
    }
}
