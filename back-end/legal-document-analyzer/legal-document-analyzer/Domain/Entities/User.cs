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

        // Profile info
        //public string? FirstName { get; set; }
        //public string? LastName { get; set; }
        //public string? PhoneNumber { get; set; }
        //public string? Address { get; set; }
        //public DateTime? Birthdate { get; set; }
        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
