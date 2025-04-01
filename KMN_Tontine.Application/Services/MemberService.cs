using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.DTOs.Responses;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Enums;
using KMN_Tontine.Domain.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace KMN_Tontine.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly UserManager<Member> _userManager;
        private readonly IMemberRepository _memberRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public MemberService(UserManager<Member> userManager, IMemberRepository memberRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _memberRepository = memberRepository;
            _httpContextAccessor = httpContextAccessor;
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
            var members = await _memberRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MemberResponse>>(members);
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

                // Mettre à jour les propriétés
                member.FullName = request.FullName ?? member.FullName;
                member.DateOfBirth = request.DateOfBirth ?? member.DateOfBirth;
                member.IsActive = request.IsActive ?? member.IsActive;

                await _memberRepository.UpdateAsync(member);
                return SimpleResponse.Ok("Member updated successfully");
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
    }
}
