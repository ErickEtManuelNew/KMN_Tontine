using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using AutoMapper;

using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Shared.Enums;
using KMN_Tontine.Domain.Interfaces;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;
using Azure.Core;

namespace KMN_Tontine.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly UserManager<Member> _userManager;
        private readonly IMemberRepository _memberRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public MemberService(UserManager<Member> userManager, IMemberRepository memberRepository,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IEmailService emailService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _memberRepository = memberRepository;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<MemberResponse> GetMemberByIdAsync(Guid id)
        {
            var member = await _userManager.FindByIdAsync(id.ToString());
            if (member == null)
                throw new KeyNotFoundException("Member not found");

            return _mapper.Map<MemberResponse>(member);
        }

        public async Task<IEnumerable<MemberResponse>> GetAllMembersAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        public async Task<SimpleResponse> CreateMemberAsync(CreateMemberRequest request)
        {
            try
            {
                // Mapper CreateMemberRequest vers Member
                var member = _mapper.Map<Member>(request);

                // Définir des propriétés supplémentaires
                member.UserName = request.Email; // Utiliser l'email comme nom d'utilisateur
                member.JoinDate = DateTime.UtcNow; // Définir automatiquement la date d'adhésion

                // Créer l'utilisateur avec Identity
                var result = await _userManager.CreateAsync(member, request.Password);
                if (!result.Succeeded)
                {
                    return SimpleResponse.Error($"Échec de la création du membre : {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                // Ajouter le rôle spécifié (ou par défaut)
                await _userManager.AddToRoleAsync(member, request.Role.ToString());

                return SimpleResponse.Ok("Membre créé avec succès.");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Échec de la création du membre : {ex.Message}");
            }
        }

        public async Task<SimpleResponse> UpdateMemberAsync(Guid id, UpdateMemberRequest request)
        {
            try
            {
                var member = await _memberRepository.GetByIdAsync(id);
                if (member == null)
                    return SimpleResponse.Error("Member not found");

                // Mettre à jour les propriétés de base
                member.FullName = request.FullName ?? member.FullName;
                member.DateOfBirth = request.DateOfBirth ?? member.DateOfBirth;

                // Vérifier si le compte vient d'être approuvé
                bool isNewlyApproved = !member.IsActive && request.IsActive == true;

                // Gérer l'approbation/rejet
                if (request.IsActive.HasValue)
                {
                    member.IsActive = request.IsActive.Value;
                }
                
                // Gérer le verrouillage (rejet)
                if (request.LockoutEnabled.HasValue)
                {
                    member.LockoutEnabled = request.LockoutEnabled.Value;
                    
                    if (request.LockoutEnd.HasValue)
                    {
                        member.LockoutEnd = request.LockoutEnd.Value;
                    }
                }
                
                // Gérer les rôles
                var currentRole = _userManager.GetRolesAsync(member).Result.FirstOrDefault();
                if (currentRole != null)
                {
                    await _userManager.RemoveFromRoleAsync(member, currentRole);
                }
                await _userManager.AddToRoleAsync(member, request.Role.ToString());

                // Si le compte est approuvé, envoyer un email avec le lien de confirmation
                if (isNewlyApproved && !member.EmailConfirmed)
                {
                    try
                    {
                        // Générer un nouveau token de confirmation
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(member);

                        // Construire le lien de confirmation
                        var confirmationLink = $"{_configuration["ApiSettings:ConfirmationUrl"]}{request.PortClient}/confirm-email?userId={member.Id}&token={WebUtility.UrlEncode(token)}";

                        // Envoyer l'email
                        var emailSent = await _emailService.SendAccountApprovedAsync(
                            member.Email,
                            member.FullName,
                            confirmationLink
                        );

                        if (!emailSent)
                        {
                            return SimpleResponse.Error("Le compte a été approuvé mais l'envoi de l'email a échoué. Veuillez réessayer l'approbation.");
                        }
                    }
                    catch (Exception ex)
                    {
                        return SimpleResponse.Error($"Le compte a été approuvé mais l'envoi de l'email a échoué : {ex.Message}");
                    }
                }

                await _memberRepository.UpdateAsync(member);

                if (isNewlyApproved)
                {
                    return SimpleResponse.Ok("Membre approuvé avec succès. Un email de confirmation a été envoyé.");
                }

                return SimpleResponse.Ok("Membre mis à jour avec succès");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to update member: {ex.Message}");
            }
        }

        public async Task<SimpleResponse> DeleteMemberAsync(Guid id)
        {
            try
            {
                var member = await _memberRepository.GetByIdAsync(id);
                if (member == null)
                    return SimpleResponse.Error("Member not found");

                await _memberRepository.DeleteAsync(id);
                return SimpleResponse.Ok("Member deleted successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to delete member: {ex.Message}");
            }
        }

        public async Task<SimpleResponse> DeactivateMemberAsync(Guid id)
        {
            // Accéder à l'utilisateur via HttpContext
            var userPrincipal = _httpContextAccessor.HttpContext?.User;
            if (userPrincipal == null)
            {
                return SimpleResponse.Error("User not authenticated");
            }

            var user = await _userManager.FindByIdAsync(id.ToString()); // Convertir Guid en string pour Identity

            if (user == null)
            {
                return new SimpleResponse { Success = false, Message = "Membre non trouvé." };
            }

            // Vérification supplémentaire : Empêcher l'auto-désactivation ? Empêcher la désactivation du dernier admin ?
            var currentUserId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user.Id.ToString() == currentUserId)
            {
                return new SimpleResponse { Success = false, Message = "Vous ne pouvez pas vous désactiver vous-même." };
            }

            try
            {
                // Alternative : Si vous avez un champ booléen IsActive
                user.IsActive = false;
                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    return new SimpleResponse { Success = true, Message = "Membre désactivé avec succès." };
                }
                else
                {
                    // Renvoyer les erreurs Identity peut exposer des détails internes, soyez prudent.
                    return new SimpleResponse { Success = false, Message = "Échec de la mise à jour du statut du membre." /*, Errors = lockoutResult.Errors.Select(e => e.Description).ToList()*/ };
                }
            }
            catch (Exception ex)
            {
                return new SimpleResponse { Success = false, Message = "Une erreur serveur est survenue." };
            }
        }

        //public async Task<SimpleResponse> ApproveMemberAsync(string memberId)
        //{
        //    try
        //    {
        //        var member = await _userManager.FindByIdAsync(memberId);
        //        if (member == null)
        //            return SimpleResponse.Error("Membre non trouvé");

        //        // Activer le compte
        //        member.IsActive = true;
                
        //        // Générer le token de confirmation d'email
        //        var token = await _userManager.GenerateEmailConfirmationTokenAsync(member);
        //        var confirmationLink = $"{_configuration["ApiSettings:BaseUrl"]}{request.PortClient}/confirm-email?userId={memberId}&token={HttpUtility.UrlEncode(token)}";

        //        // Envoyer l'email
        //        var emailSent = await _emailService.SendAccountApprovedAsync(
        //            member.Email,
        //            member.FullName,
        //            confirmationLink
        //        );

        //        if (!emailSent)
        //        {
        //            return SimpleResponse.Error("Erreur lors de l'envoi de l'email de confirmation");
        //        }

        //        await _userManager.UpdateAsync(member);
        //        return SimpleResponse.Ok("Membre approuvé avec succès");
        //    }
        //    catch (Exception ex)
        //    {
        //        return SimpleResponse.Error($"Erreur lors de l'approbation : {ex.Message}");
        //    }
        //}

        //public async Task<SimpleResponse> RejectMemberAsync(string memberId, string reason)
        //{
        //    try
        //    {
        //        var member = await _userManager.FindByIdAsync(memberId);
        //        if (member == null)
        //            return SimpleResponse.Error("Membre non trouvé");

        //        // Verrouiller le compte
        //        member.LockoutEnabled = true;
        //        member.LockoutEnd = DateTimeOffset.MaxValue;

        //        // Envoyer l'email de rejet
        //        var emailSent = await _emailService.SendAccountRejectedAsync(
        //            member.Email,
        //            member.FullName,
        //            reason
        //        );

        //        if (!emailSent)
        //        {
        //            return SimpleResponse.Error("Erreur lors de l'envoi de l'email de rejet");
        //        }

        //        await _userManager.UpdateAsync(member);
        //        return SimpleResponse.Ok("Membre rejeté avec succès");
        //    }
        //    catch (Exception ex)
        //    {
        //        return SimpleResponse.Error($"Erreur lors du rejet : {ex.Message}");
        //    }
        //}
    }
}
