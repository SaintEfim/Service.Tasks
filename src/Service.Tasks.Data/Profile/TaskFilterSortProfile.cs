using Service.Tasks.Data.Models;
using Sieve.Services;

namespace Service.Tasks.Data.Profile;

public class TaskFilterSortProfile : ISieveConfiguration
{
    public void Configure(
        SievePropertyMapper mapper)
    {
        mapper.Property<TaskEntity>(p => p.Id)
            .CanFilter();

        mapper.Property<TaskEntity>(p => p.Title)
            .CanFilter()
            .CanSort();

        mapper.Property<TaskEntity>(p => p.Description)
            .CanFilter()
            .CanSort();

        mapper.Property<TaskEntity>(p => p.ParentId)
            .CanFilter();
    }
}
