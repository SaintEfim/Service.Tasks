using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.Base;

/// <summary>
/// Represents the result of a create operation.
/// </summary>
public class CreateActionResultDto : IDto
{
    /// <summary>
    /// Gets or sets the ID of the created entity.
    /// </summary>
    [Required]
    public Guid Id { get; set; }
}
