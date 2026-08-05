using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain.Unidades;

public enum UnidadesEnum
{
    [Display(Name ="GERENCIA")]
    GERENCIA = 1,
    [Display(Name ="UNIDAD ADMINISTRATIVA")]
    UA = 2,
    [Display(Name ="UNIDAD DE ASUNTOS JURÍDICOS")]
    UAJ= 3,
    [Display(Name ="UNIDAD TÉCNICA DE SEGUIMIENTO Y EVALUACIÓN")]
    UTSE = 4,
    [Display(Name ="UNIDAD DE AUDITORÍA INTERNA")]
    UDAI = 5
}