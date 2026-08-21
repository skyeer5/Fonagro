namespace WebApp.Domain.Comisiones;

public enum ComisionEstados
{
    Creada = 1, // Cuando la crea servicios 
    DestinosDefinidos = 2,// cuando se ponen los destinos
    Programada = 3, // cuando ya queda aprobada y lista para continuar
    EnCurso = 4, // cuando empieza la comision
    Completada = 5, // cuando termina la comison
    Cancelada = 6 // lo que dice
}