using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Tasks.API.Models.Base;
using Service.Tasks.Domain.Models.Base;
using Service.Tasks.Domain.Services.Base;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.API.Controllers.Base;

[ApiController]
public abstract class ControllerCrudBase<TDto, TModel, TManager, TProvider> : ControllerBase
    where TDto : class, IDto
    where TModel : class, IModel
    where TManager : IDataManager<TModel>
    where TProvider : IDataProvider<TModel>
{
    protected ControllerCrudBase(
        IMapper mapper,
        TManager manager,
        TProvider provider)
    {
        Mapper = mapper;
        Manager = manager;
        Provider = provider;
    }

    protected TManager Manager { get; }

    protected TProvider Provider { get; }

    protected IMapper Mapper { get; }

    protected async Task<IEnumerable<TDto>> Get(
        FilterSettings? filter,
        bool withIncludes = false,
        CancellationToken cancellationToken = default)
    {
        var models = await Provider.Get(filter, withIncludes, cancellationToken);
        return Mapper.Map<IEnumerable<TDto>>(models);
    }

    protected async Task<TDto?> GetOneById(
        Guid id,
        bool withIncludes = false,
        CancellationToken cancellationToken = default)
    {
        var model = await Provider.GetOneById(id, withIncludes, cancellationToken);
        return Mapper.Map<TDto?>(model);
    }

    protected async Task<IActionResult> Create<TCreateDto, TCreatedResultDto>(
        TCreateDto createDto,
        string getByIdRouteName,
        CancellationToken cancellationToken = default)
        where TCreateDto : class
    {
        var model = Mapper.Map<TModel>(createDto);
        var created = await Manager.Create(model, cancellationToken);
        return CreatedAtRoute(getByIdRouteName, new { id = created.Id }, Mapper.Map<TCreatedResultDto>(created));
    }

    protected async Task<IActionResult> Update<TUpdateDto>(
        Guid id,
        JsonPatchDocument<TUpdateDto> patchDocument,
        CancellationToken cancellationToken = default)
        where TUpdateDto : class
    {
        var model = await Provider.GetOneById(id, true, cancellationToken);
        var updateDto = Mapper.Map<TUpdateDto>(model);

        patchDocument.ApplyTo(updateDto);

        if (!TryValidateModel(updateDto))
        {
            return BadRequest(ModelState);
        }

        Mapper.Map(updateDto, model);
        var updated = await Manager.Update(model, cancellationToken);

        return Ok(Mapper.Map<TDto>(updated));
    }

    protected async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await Manager.Delete(id, cancellationToken);
        return NoContent();
    }
}
