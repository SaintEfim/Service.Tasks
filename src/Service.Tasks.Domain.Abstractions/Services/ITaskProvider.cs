using Service.Tasks.Domain.Models;
using Service.Tasks.Domain.Services.Base;

namespace Service.Tasks.Domain.Services;

public interface ITaskProvider : IDataProvider<TaskModel>;
