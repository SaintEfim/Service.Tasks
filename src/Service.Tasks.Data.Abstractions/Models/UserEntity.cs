using Service.Tasks.Data.Models.Base;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.Data.Models;

public class UserEntity : EntityBase
{
    public string UserName { get; set; } = string.Empty;

    public RoleEnum Role { get; set; }

    public string Password { get; set; } = string.Empty;
}
