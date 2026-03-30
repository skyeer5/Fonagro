namespace WebApp.Application.Authentication.Command.ChangePassword;

public class ChangePasswordRequest
{
    public string? AnteriorPassword { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
}