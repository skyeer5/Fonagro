using MediatR;
using WebApp.Application.Core;
using WebApp.Application.Interfaces;
using WebApp.Domain;
using static WebApp.Application.Comisiones.Command.ComisionAddDestinos.ComisionAddDestinosCommand;

namespace WebApp.Application.Comisiones.Command.ComisionAddDestinos;

public class ComisionAddDestinosCommandHandler : IRequestHandler<ComisionAddDestinosCommandRequest, Result<int>>
{
    private readonly IComisionService _comisionService;
    private readonly IComisionRepository _comisionRepository;

    public ComisionAddDestinosCommandHandler(IComisionService comisionService, IComisionRepository comisionRepository)
    {
        _comisionService = comisionService;
        _comisionRepository = comisionRepository;
    }

    public async Task<Result<int>> Handle(ComisionAddDestinosCommandRequest request, CancellationToken cancellationToken)
    {
        var comision = await  _comisionService.GetComisionByIdAsync(request.request.ComisionId);

        if(comision is null)
        {
            return Result<int>.Failure("No se ha encontrado la comision");
        }

        var destinos = new List<ComisionDestino>();
        foreach(var com in request.request.comisionAddDestinosItemRequests!)
        {
            var destino = ComisionDestino.Crear(com.Descripcion!, com.Kilometro, comision.Vehiculo!.ConsumoKmPorGalon);
            destinos.Add(destino);
        }
        
        comision.AgregarDestinos(destinos);

         var resultado = await _comisionRepository.AddDestinosAsync(comision, cancellationToken);

         return resultado.IsSuccess ? Result<int>.Success(resultado.Value) : Result<int>.Failure(resultado.Error!);
    }
}