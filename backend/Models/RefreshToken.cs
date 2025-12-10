using Eventra.Models;

namespace Eventra.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;      // O token em si (GUID ou string aleatória)
        public DateTime ExpiresAt { get; set; }                 // Quando expira
        public DateTime CreatedAt { get; set; }                 // Quando foi criado
        public DateTime? RevokedAt { get; set; }                // Se foi revogado (logout)
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt != null;
        public bool IsActive => !IsRevoked && !IsExpired;
        
        // Relacionamento
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}