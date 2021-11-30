﻿using System.Linq;
using System.Threading.Tasks;
using DiFY.Modules.UserAccess.Application.Contracts;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace DiFY.Modules.UserAccess.Application.IdentityServer
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.AddRange(context.Subject.Claims.Where(claim => claim.Type == CustomClaimTypes.Roles).ToList());
            context.IssuedClaims.Add(context.Subject.Claims.Single(claim => claim.Type == CustomClaimTypes.Name));
            context.IssuedClaims.Add(context.Subject.Claims.Single(claim => claim.Type == CustomClaimTypes.Email));
            
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(context.IsActive);
        }
    }
}