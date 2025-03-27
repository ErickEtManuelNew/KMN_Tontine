using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Enums;

namespace KMN_Tontine.Application.DTOs.Requests
{
    public class RegisterRequest
    {
        /// <summary>
        /// Le prénom du membre (obligatoire).
        /// </summary>
        [Required(ErrorMessage = "Le prénom est requis.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Le nom de famille du membre (obligatoire).
        /// </summary>
        [Required(ErrorMessage = "Le nom de famille est requis.")]
        public string LastName { get; set; }

        /// <summary>
        /// La date de naissance du membre (obligatoire).
        /// </summary>
        [Required(ErrorMessage = "La date de naissance est requise.")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// L'adresse email du membre, utilisée comme identifiant unique (obligatoire).
        /// </summary>
        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "L'email doit être une adresse email valide.")]
        public string Email { get; set; }

        /// <summary>
        /// Le mot de passe du membre (obligatoire).
        /// </summary>
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
        public string Password { get; set; }

        /// <summary>
        /// Le rôle du membre dans le système (par défaut : Member).
        /// </summary>
        public RoleType Role { get; set; } = RoleType.Member;

        public string Address { get; set; }
    }
}
