using Service.Tasks.Data.Models.Base;

namespace Service.Tasks.Data.Models;

public class ExecutorEntity : EntityBase
{
    public string Name { get; set; } = string.Empty;
}
