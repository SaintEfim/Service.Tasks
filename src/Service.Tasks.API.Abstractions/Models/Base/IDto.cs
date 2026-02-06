namespace Service.Tasks.API.Models.Base;

/// <summary>
/// Interface for all Data Transfer Objects (DTOs).
/// </summary>
public interface IDto
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    Guid Id { get; set; }
}
