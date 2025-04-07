using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums.Negocio;
using Uno;

namespace Application.Services.NegocioS
{
    public class NegocioService
    {
        private readonly IReadOnlyRepositoryAsync<Negocio> _negocioReadingRepository;
        private readonly IRepositoryAsync<Negocio> _negocioRepository;

        public NegocioService(
            IReadOnlyRepositoryAsync<Negocio> negocioReadingRepository,
            IRepositoryAsync<Negocio> negocioRepository)
        {
            _negocioReadingRepository = negocioReadingRepository;
            _negocioRepository = negocioRepository;
        }

        public async Task<string> DeterminarNegocio(long negocioId, bool aprobado)
        {
            var negocio = await _negocioReadingRepository.GetByIdAsync(negocioId);
            if (negocio == null)
                throw new ApiException("El negocio no existe.");

            negocio.estado = aprobado ? Estado.Activo : Estado.Inactivo;

            await _negocioRepository.UpdateAsync(negocio);
            await _negocioRepository.SaveChangesAsync();

            return aprobado
                ? "✅ El negocio ha sido aprobado correctamente."
                : "❌ El negocio ha sido rechazado correctamente.";
        }
    }
}