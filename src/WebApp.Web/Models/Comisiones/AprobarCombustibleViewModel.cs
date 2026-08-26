using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Application.Comisiones.Command.ComisionApprovalGas;
using WebApp.Application.Comisiones.Queries.GetComisionesPendApprov;

namespace WebApp.Web.Models.Comisiones;

public class AprobarCombustibleViewModel : ComisionApprovalGasRequest
{
    public List<GetComisionesPendApprovResponse> ComisionesList { get; set; } = [];
    public List<SelectListItem> EstadosList { get; set; } =
    [
        new() {
            Text = "Pendientes de aprobación",
            Value = "1"
        },
        new() {
            Text = "Aprobadas",
            Value = "2"
        }, 
        new() {
            Text = "Todas",
            Value = "3"
        }  
    ];
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}