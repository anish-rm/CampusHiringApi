using System.ComponentModel.DataAnnotations;

namespace CampusHiring.Api.Application.DTOs.Auth;

public class RegisteredUserDto
{
    public required string Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role {  get; set; } = string.Empty;
}
