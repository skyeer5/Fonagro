using System.ComponentModel;

namespace WebApp.Domain.enums;

public enum Unidades
{
    [Description("Unidad Administrativa")]
    UA = 1,
    [Description("Unidad de Auditoria Interna")]
    UDAI = 2,
    [Description("Unidad Técnica de Seguimiento y Evaluación")]
    UTSE= 3,
    [Description("Unidad de Asuntos Jurídicos")]
    UAJ = 4,
    [Description("Unidad Desconcentrada de Administración Financiera")]
    UDDAF = 5
}