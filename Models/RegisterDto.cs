namespace multitenant_app.Models
{
    public class RegisterDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? DatabaseName { get; set; }
    }
}
