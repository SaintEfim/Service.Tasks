using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Service.Tasks.API.Controllers.Base;
using Service.Tasks.API.Models.Base;
using Service.Tasks.API.Models.Task;
using Service.Tasks.Domain.Models.Task;
using Service.Tasks.Domain.Services.Task;
using Service.Tasks.Shared.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Service.Tasks.API.Controllers;

/// <summary>
/// Controller for managing task entities.
/// Provides CRUD operations for tasks with support for hierarchical task structures.
/// </summary>
[Route("api/v1/Tasks")]
public sealed class TaskController : ControllerCrudBase<TaskDto, TaskModel, ITaskManager, ITaskProvider>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskController"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="manager">The task manager service for business logic operations.</param>
    /// <param name="provider">The task provider service for data retrieval operations.</param>
    public TaskController(
        IMapper mapper,
        ITaskManager manager,
        ITaskProvider provider)
        : base(mapper, manager, provider)
    {
    }

    /// <summary>
    /// Retrieves a list of tasks with optional filtering and includes.
    /// Requires user authentication.
    /// </summary>
    /// <param name="filter">Filtering criteria for task retrieval.</param>
    /// <param name="withIncludes">Indicates whether to include related entities.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A list of <see cref="TaskDto"/> objects matching the filter criteria.
    /// Returns HTTP 200 on success.
    /// </returns>
    [HttpGet]
    [Authorize]
    [OpenApiOperation(nameof(TaskGet))]
    [SwaggerResponse(Status200OK, typeof(List<TaskDto>))]
    public async Task<ActionResult<List<TaskDto>>> TaskGet(
        [FromQuery] FilterSettings filter,
        bool withIncludes = false,
        CancellationToken cancellationToken = default)
    {
        return Ok(await Get(filter, withIncludes, cancellationToken));
    }

    /// <summary>
    /// Retrieves a hierarchical tree structure of tasks.
    /// If rootId is provided, returns the tree starting from the specified task.
    /// If rootId is null, returns all root-level task trees.
    /// Requires user authentication.
    /// </summary>
    /// <param name="rootId">Optional root task ID to build tree from. If null, returns all root tasks.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A hierarchical tree of <see cref="TaskDto"/> objects.
    /// Returns HTTP 200 on success, HTTP 404 if the root task is not found.
    /// </returns>
    [HttpGet("tree", Name = nameof(TaskBuildTree))]
    [Authorize]
    [OpenApiOperation(nameof(TaskBuildTree))]
    [SwaggerResponse(Status200OK, typeof(TaskDto))]
    [SwaggerResponse(Status404NotFound, typeof(ErrorDto))]
    public async Task<IActionResult> TaskBuildTree(
        [FromQuery] Guid? rootId = null,
        CancellationToken cancellationToken = default)
    {
        var data = await Provider.ExportTree(rootId, cancellationToken: cancellationToken);

        return Ok(Mapper.Map<List<TaskDto>>(data));
    }

    /// <summary>
    /// Retrieves a single task by its unique identifier.
    /// Includes related entities by default.
    /// Requires user authentication.
    /// </summary>
    /// <param name="id">The unique identifier of the task to retrieve.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// The requested <see cref="TaskDto"/> object.
    /// Returns HTTP 200 on success, HTTP 404 if the task is not found.
    /// </returns>
    [HttpGet("{id:guid}", Name = nameof(TaskGetById))]
    [Authorize]
    [OpenApiOperation(nameof(TaskGetById))]
    [SwaggerResponse(Status200OK, typeof(TaskDto))]
    [SwaggerResponse(Status404NotFound, typeof(ErrorDto))]
    public async Task<ActionResult<TaskDto>> TaskGetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return Ok(await GetOneById(id, true, cancellationToken));
    }

    /// <summary>
    /// Creates a new task entity.
    /// Requires Admin role authorization.
    /// </summary>
    /// <param name="payload">The task creation data transfer object.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A <see cref="CreateActionResultDto"/> containing the created task's ID and location header.
    /// Returns HTTP 201 on successful creation, HTTP 400 for invalid request data.
    /// </returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [OpenApiOperation(nameof(TaskCreate))]
    [SwaggerResponse(Status201Created, typeof(CreateActionResultDto))]
    [SwaggerResponse(Status400BadRequest, typeof(ErrorDto))]
    public Task<IActionResult> TaskCreate(
        [FromBody] TaskCreateDto payload,
        CancellationToken cancellationToken = default)
    {
        return Create<TaskCreateDto, CreateActionResultDto>(payload, nameof(TaskGetById), cancellationToken);
    }

    /// <summary>
    /// Updates an existing task using JSON Patch operations.
    /// Supports partial updates of task properties.
    /// Requires Admin role authorization.
    /// </summary>
    /// <param name="id">The unique identifier of the task to update.</param>
    /// <param name="patchDocument">The JSON Patch document containing update operations.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// HTTP 200 on successful update.
    /// Returns HTTP 400 for invalid patch operations, HTTP 404 if the task is not found.
    /// </returns>
    /// <remarks>
    /// Example patch operations:
    /// [
    ///   { "op": "replace", "path": "/title", "value": "New Task Title" },
    ///   { "op": "replace", "path": "/description", "value": "Updated description" }
    /// ]
    /// </remarks>
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [OpenApiOperation(nameof(TaskUpdate))]
    [SwaggerResponse(Status200OK, typeof(void))]
    [SwaggerResponse(Status404NotFound, typeof(ErrorDto))]
    [SwaggerResponse(Status400BadRequest, typeof(ErrorDto))]
    public async Task<IActionResult> TaskUpdate(
        Guid id,
        [FromBody] JsonPatchDocument<TaskUpdateDto> patchDocument,
        CancellationToken cancellationToken = default)
    {
        return await Update(id, patchDocument, cancellationToken);
    }

    /// <summary>
    /// Deletes a task by its unique identifier.
    /// Requires Admin role authorization.
    /// </summary>
    /// <param name="id">The unique identifier of the task to delete.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// HTTP 204 on successful deletion.
    /// Returns HTTP 400 if deletion is not allowed, HTTP 404 if the task is not found.
    /// </returns>
    /// <remarks>
    /// Deleting a task may cascade to its child tasks based on database configuration.
    /// </remarks>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [OpenApiOperation(nameof(TaskDelete))]
    [SwaggerResponse(Status204NoContent, typeof(void))]
    [SwaggerResponse(Status400BadRequest, typeof(ErrorDto))]
    [SwaggerResponse(Status404NotFound, typeof(ErrorDto))]
    public async Task<IActionResult> TaskDelete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await Delete(id, cancellationToken);
    }
}
