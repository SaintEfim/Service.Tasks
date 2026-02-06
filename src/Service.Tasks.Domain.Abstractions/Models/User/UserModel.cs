using Service.Tasks.Domain.Models.Base;
using Service.Tasks.Shared.Models;

namespace Service.Tasks.Domain.Models.User;

public class UserModel : ModelBase
{
    public string UserName { get; set; } = string.Empty;

    public RoleEnum Role { get; set; }

    public string Password { get; set; } = string.Empty;
}
