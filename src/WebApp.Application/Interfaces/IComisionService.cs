using WebApp.Application.Comisiones.Queries.GetComisionesActivas;

namespace WebApp.Application.Interfaces;

public interface IComisionService
{
    Task<GetComisionActivaResponse?> GetComisionActivaAsync(int usuarioId);
    Task<Domain.Comision?> GetComisionByIdAsync(int comisionId);
}