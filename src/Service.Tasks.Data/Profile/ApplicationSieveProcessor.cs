using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace Service.Tasks.Data.Profile;

public class ApplicationSieveProcessor : SieveProcessor
{
    public ApplicationSieveProcessor(
        IOptions<SieveOptions> options)
        : base(options)
    {
    }

    protected override SievePropertyMapper MapProperties(
        SievePropertyMapper mapper)
    {
        return mapper.ApplyConfiguration<TaskFilterSortProfile>()
            .ApplyConfiguration<UserFilterSortProfile>();
    }
}
