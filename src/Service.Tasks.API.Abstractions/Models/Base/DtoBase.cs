using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.Base;

/// <summary>
/// Base class for all Data Transfer Objects (DTOs).
/// </summary>
public class DtoBase : IDto
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    [Required]
    public required Guid Id { get; set; }
}
