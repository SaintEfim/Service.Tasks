using AutoMapper;
using Service.Task.Domain.Models;
using Service.Task.Domain.Services.Base;
using Service.Tasks.Data.Models;
using Service.Tasks.Data.Repositories;

namespace Service.Task.Domain.Services.Executor;

public class ExecutorManager : DataManagerBase<ExecutorModel, ExecutorEntity, IExecutorRepository>
{
    public ExecutorManager(
        IMapper mapper,
        IExecutorRepository repository)
        : base(mapper, repository)
    {
    }
}
