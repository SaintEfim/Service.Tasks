using Service.Tasks.Domain.Models;
using Service.Tasks.Domain.Services.Base;

namespace Service.Tasks.Domain.Services.Task;

public interface ITaskManager : IDataManager<TaskModel>;
