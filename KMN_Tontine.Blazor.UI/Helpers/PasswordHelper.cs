using Microsoft.AspNetCore.Identity;

namespace KMN_Tontine.Blazor.UI.Helpers
{
    public class PasswordHelper
    {
        private readonly PasswordHasher<string> _passwordHasher = new();

        // Méthode pour hasher un mot de passe
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        // Méthode pour vérifier un mot de passe
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
