namespace legal_document_analyzer.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }

        // Auth info
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; } // Nullable for security reasons
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? RefreshToken { get; set; } // For JWT refresh token
    }
}
