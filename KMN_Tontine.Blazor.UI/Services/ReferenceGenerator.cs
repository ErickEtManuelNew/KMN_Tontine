using System;
using System.Text;
using System.Linq;

namespace KMN_Tontine.Blazor.UI.Services
{
    public class ReferenceGenerator
    {
        private static readonly Random random = new Random();
        private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GeneratePromiseReference()
        {
            // Générer une référence aléatoire de 7 caractères
            return new string(Enumerable.Repeat(chars, 7)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
} 