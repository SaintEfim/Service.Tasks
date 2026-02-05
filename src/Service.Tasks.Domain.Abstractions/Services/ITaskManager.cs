using Service.Task.Domain.Services.Base;
using Service.Tasks.Domain.Models;

namespace Service.Tasks.Domain.Services;

public interface ITaskManager : IDataManager<TaskModel>;
