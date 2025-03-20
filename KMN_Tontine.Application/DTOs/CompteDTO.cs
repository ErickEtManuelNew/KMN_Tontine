using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KMN_Tontine.Application.DTOs
{
    public class CompteDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du compte est requis")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le solde est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le solde doit être positif")]
        public decimal Solde { get; set; }

        public string? Notes { get; set; }

        public string MembreId { get; set; } = string.Empty;
    }
}
