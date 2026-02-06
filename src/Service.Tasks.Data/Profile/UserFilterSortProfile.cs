using Service.Tasks.Data.Models;
using Sieve.Services;

namespace Service.Tasks.Data.Profile;

public class UserFilterSortProfile : ISieveConfiguration
{
    public void Configure(
        SievePropertyMapper mapper)
    {
        mapper.Property<UserEntity>(p => p.Id)
            .CanFilter();

        mapper.Property<UserEntity>(p => p.UserName)
            .CanFilter()
            .CanSort();

        mapper.Property<UserEntity>(p => p.Password)
            .CanFilter()
            .CanSort();
    }
}
