using Service.Tasks.Data.Models.Base;

namespace Service.Tasks.Data.Models;

public class UserEntity : EntityBase
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
