using System.ComponentModel.DataAnnotations;

namespace Service.Tasks.API.Models.Base;

/// <summary>
/// Represents an error response.
/// </summary>
public class ErrorDto
{
    /// <summary>
    /// Gets or sets the error title.
    /// </summary>
    [Required]
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the error description.
    /// </summary>
    [Required]
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code.
    /// </summary>
    [Required]
    public required int StatusCode { get; set; }
}
