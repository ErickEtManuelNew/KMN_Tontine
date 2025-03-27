﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Enums;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace KMN_Tontine.Application.Seed
{
    public class SuperAdminSeeder
    {
        private readonly UserManager<Member> _userManager;
        private readonly IConfiguration _configuration;

        public SuperAdminSeeder(UserManager<Member> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task SeedSuperAdminAsync()
        {
            var email = _configuration["DefaultSuperAdmin:Email"];
            var password = _configuration["DefaultSuperAdmin:Password"];
            var fullName = _configuration["DefaultSuperAdmin:FullName"];
            var firstName = _configuration["DefaultSuperAdmin:FirstName"];
            var lastName = _configuration["DefaultSuperAdmin:LastName"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return;

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                var user = new Member
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    Role = RoleType.SuperAdmin,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleType.SuperAdmin.ToString());
                }
                else
                {
                    Console.WriteLine("Failed to create SuperAdmin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
