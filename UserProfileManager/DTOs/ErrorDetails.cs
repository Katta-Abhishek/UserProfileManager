namespace UserProfileManager.DTOs;

public class ErrorDetails
{
  public int StatusCode { get; set; }
  public string Message { get; set; } = string.Empty;
  public string Detail { get; set; } = string.Empty;

  public override string ToString()
  {
    return System.Text.Json.JsonSerializer.Serialize(this);
  }
}
