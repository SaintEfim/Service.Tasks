using Service.Tasks.Data.Models;
using Sieve.Services;

namespace Service.Tasks.Data.Profile;

public class ExecutorFilterSortProfile : ISieveConfiguration
{
    public void Configure(
        SievePropertyMapper mapper)
    {
        mapper.Property<ExecutorEntity>(p => p.Id)
            .CanFilter();

        mapper.Property<ExecutorEntity>(p => p.Name)
            .CanFilter()
            .CanSort();
    }
}
