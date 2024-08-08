namespace UserProfileManager.DTOs;

public class UserDTO
{
  public int Id { get; set; }
  public required string UniqueName { get; set; }
  public required string Password { get; set; }
  public required string FirstName { get; set; }
  public required string CountryCode { get; set; }
  public required string PhoneNumber { get; set; }
  public required char Gender { get; set; }
  public required string Email { get; set; }
  public required DateTime DateOfBirth { get; set; }
  public string? MiddleName { get; set; }
  public string? LastName { get; set; }
  public string? Education { get; set; }
  public string? Address { get; set; }
  public byte[]? Photo { get; set; }
  public string? City { get; set; }
}