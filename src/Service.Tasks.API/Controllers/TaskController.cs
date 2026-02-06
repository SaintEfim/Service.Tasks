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

[Route("api/v1/Tasks")]
public class TaskController : ControllerCrudBase<TaskDto, TaskModel, ITaskManager, ITaskProvider>
{
    public TaskController(
        IMapper mapper,
        ITaskManager manager,
        ITaskProvider provider)
        : base(mapper, manager, provider)
    {
    }

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
