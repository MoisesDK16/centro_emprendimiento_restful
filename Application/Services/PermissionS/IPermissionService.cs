using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Azure.Core;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.PermissionS
{
    public interface IPermissionService
    {
        Task ValidateNegocioPermission(long negocioId, string userId);

    }
}
