using AutoMapper;
using Service.Task.Domain.Models;
using Service.Task.Domain.Services.Base;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories;

namespace Service.Task.Domain.Services.Executor;

public class ExecutorProvider : DataProviderBase<ExecutorModel, ExecutorEntity, IExecutorRepository>
{
    public ExecutorProvider(
        IMapper mapper,
        IExecutorRepository repository)
        : base(mapper, repository)
    {
    }
}
