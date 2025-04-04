using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Azure.Core;
using Domain.Entities;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.PermissionS
{
    public class PermissionService : IPermissionService
    {
        private readonly IReadOnlyRepositoryAsync<Negocio> _readingNegocio;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionService(IReadOnlyRepositoryAsync<Negocio> readingNegocio, UserManager<ApplicationUser> userManager)
        {
            _readingNegocio = readingNegocio;
            _userManager = userManager;
        }

        public async Task ValidateNegocioPermission(long negocioId, string userId)
        {
            switch(await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(userId)))
            {
                case var roles when roles.Contains("Admin"):
                    _ = await _readingNegocio.FirstOrDefaultAsync(
                        new NegocioSpecification(negocioId, userId, false, true))
                        ?? throw new ApiException("No está permitido interactuar con la información de otros emprendimientos aparte de los suyos.");
                    return;
                case var roles when roles.Contains("Vendedor"):
                    _ = await _readingNegocio.FirstOrDefaultAsync(
                        new NegocioSpecification(negocioId, userId, true))
                        ?? throw new ApiException("No está permitido interactuar con la información de otros emprendimientos aparte de los suyos.");
                    return;
                default:
                    _ = await _readingNegocio.FirstOrDefaultAsync(
                        new NegocioSpecification(negocioId, userId))
                        ?? throw new ApiException("No está permitido interactuar con la información de otros emprendimientos aparte de los suyos.");
                    return;
            }
        }
    }
}
