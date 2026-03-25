namespace WebApp.Domain;

public static class UsuariosTipos
{
    //Puestos
    public const string AUXILIAR = nameof(AUXILIAR);
    public const string ASISTENTE = nameof(ASISTENTE);
    public const string AUDITOR = nameof(AUDITOR);
    public const string ASESOR = nameof(ASESOR);
    public const string ENCARGADO = nameof(ENCARGADO);
    public const string SUBCOORDINADOR = nameof(SUBCOORDINADOR);
    public const string COORDINADOR = nameof(COORDINADOR);
    public const string GERENTE = "GERENTE GENERAL";
    // Encargados
    public const string Encargado_Administracion = "ENCARGADO DE ADMINISTRACION Y PERSONAL";
    public const string Encargado_Servicios = "ENCARGADO DE SERVICIOS GENERALES";
    public const string Encargado_Sistemas = "ENCARGADO DE SISTEMAS";
    public const string Encargado_Archivo = "ENCARGADO DE ARCHIVO";
    public const string Encargado_Info_Publica = "ENCARGADO DE COMUNICACION E INFORMACION PUBLICA";
    public const string Encargado_Planificacion = "ENCARGADO DE PLANIFICACION";
    public const string Encargado_Recuperacion = "ENCARGADO DE RECUPERACION DE CARTERA";

    // Auxiliares
    public const string Auxiliar_Servicios = "AUXILIAR DE SERVICIOS GENERALES";
    public const string Auxiliar_Sistemas = "AUXILIAR DE SISTEMAS";
    public const string Auxiliar_Archivo = "AUXILIAR DE ARCHIVO";
    public const string Auxiliar_Info_Publica = "AUXILIAR DE COMUNICACION E INFORMACION PUBLICA";
    public const string Piloto_Mensajero = "PILOTO MENSAJERO";
    
    //Asesores
    public const string ASESOR_ADMON = "ASESOR DE LA UNIDAD ADMINISTRATIVA";
    public const string ASESOR_UAJ = "ASESOR JURIDICO";
    public const string ASESOR_UTSE = "ASESOR DE EVALUACION, SEGUIMIENTO Y LIQUIDACION DE PROYECTOS";
    public const string ASESOR_UDAI = "ASESOR DE LA UNIDAD DE AUDITORIA INTERNA";

    //Asistentes
    public const string ASISTENTE_ADMON = "ASISTENTE DE LA UNIDAD ADMINISTRATIVA";
    public const string ASISTENTE_UAJ = "ASISTENTE DE ASESORIA JURIDICA";
    public const string ASISTENTE_UTSE = "ASISTENTE DE LA UNIDAD TECNICA DE SEGUIMIENTO Y EVALUACION";
    public const string ASISTENTE_UDAI = "ASISTENTE DE LA UNIDAD DE AUDITORIA INTERNA";

    //Coordinadores
    public const string COORDINADOR_ADMON = "COORDINADOR DE LA UNIDAD ADMINISTRATIVA";
    public const string COORDINADOR_UAJ = "COORDINADOR DE LA UNIDAD DE ASESORIA JURIDICA";
    public const string COORDINADOR_UTSE = "COORDINADOR DE LA UNIDAD TECNICA DE SEGUIMIENTO Y EVALUACION";
    public const string COORDINADOR_UDAI = "COORDINADOR DE LA UNIDAD DE AUDITORIA INTERNA";
    //Subcoordinadores
    public const string SUBCOORDINADOR_ADMON = "SUBCOORDINADOR DE LA UNIDAD ADMINISTRATIVA";
    public const string SUBCOORDINADOR_UAJ = "SUBCOORDINADOR DE LA UNIDAD DE ASESORIA JURIDICA";
    public const string SUBCOORDINADOR_UTSE = "SUBCOORDINADOR DE LA UNIDAD TECNICA DE SEGUIMIENTO Y EVALUACION";
    public const string SUBCOORDINADOR_UDAI = "SUBCOORDINADOR DE LA UNIDAD DE AUDITORIA INTERNA";


    //Unidades
    public const string Gerencia = nameof(Gerencia);
    public const string UA = "UNIDAD ADMINISTRATIVA";
    public const string UAJ = "UNIDAD DE ASESORIA JURIDICA";
    public const string UTSE = "UNIDAD TECNICA DE SEGUIMIENTO Y EVALUACION";
    public const string UDAI = "UNIDAD DE AUDITORIA INTERNA";

    //Tipo de servicios
    public const string Tecnicos = "SERVICIOS TECNICOS";
    public const string Profesionales = "SERICIOS PROFESIONALES";
    public static List<string> GetPuestos()
    {
        return new List<string>
        {
            AUXILIAR,
            ASISTENTE,
            AUDITOR,
            ASESOR,
            ENCARGADO,
            SUBCOORDINADOR,
            COORDINADOR,
            GERENTE
        };
    }
    public static List<string> GetEncargados()
    {
        return new List<string>
        {
            Encargado_Administracion,
            Encargado_Servicios,
            Encargado_Sistemas,
            Encargado_Archivo,
            Encargado_Info_Publica,
            Encargado_Planificacion,
            Encargado_Recuperacion
        };
    }
    public static List<string> GetAuxiliares()
    {
        return new List<string>
        {
            Auxiliar_Servicios,
            Auxiliar_Sistemas,
            Auxiliar_Archivo,
            Auxiliar_Info_Publica,
            Piloto_Mensajero
        };
    }
    public static List<string> GetUnidades()
    {
        return new List<string>
        {
            Gerencia,
            UA,
            UAJ,
            UTSE,
            UDAI
        };
    }
    public static List<string> GetTipoServicios()
    {
        return new List<string>
        {
            Tecnicos,
            Profesionales
        };
    }
}
