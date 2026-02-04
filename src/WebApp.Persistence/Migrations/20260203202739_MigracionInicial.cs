using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accesorios",
                columns: table => new
                {
                    AccesorioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Creado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modificado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accesorios", x => x.AccesorioId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_Completo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NIT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Puesto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo_Servicios = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numero_Contrato = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "gasolinas",
                columns: table => new
                {
                    GasolinaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Creado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modificado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gasolinas", x => x.GasolinaId);
                });

            migrationBuilder.CreateTable(
                name: "partes",
                columns: table => new
                {
                    ParteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Creado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modificado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partes", x => x.ParteId);
                });

            migrationBuilder.CreateTable(
                name: "viaticos",
                columns: table => new
                {
                    ViaticoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Vigente = table.Column<bool>(type: "bit", nullable: true),
                    Creado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modificado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_viaticos", x => x.ViaticoId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gasolinaPrecios",
                columns: table => new
                {
                    GasolinaPrecioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GasolinaId = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Creado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modificado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gasolinaPrecios", x => x.GasolinaPrecioId);
                    table.ForeignKey(
                        name: "FK_gasolinaPrecios_gasolinas_GasolinaId",
                        column: x => x.GasolinaId,
                        principalTable: "gasolinas",
                        principalColumn: "GasolinaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehiculos",
                columns: table => new
                {
                    VehiculoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Tipo_Vehiculo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cilindraje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kilometraje = table.Column<double>(type: "float", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GasolinaId = table.Column<int>(type: "int", nullable: false),
                    Creado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modificado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehiculos", x => x.VehiculoId);
                    table.ForeignKey(
                        name: "FK_vehiculos_gasolinas_GasolinaId",
                        column: x => x.GasolinaId,
                        principalTable: "gasolinas",
                        principalColumn: "GasolinaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comisiones",
                columns: table => new
                {
                    ComisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha_Salida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fecha_Regreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Precio_Galon_Usado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Galon_Estimado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Presupuesto_Combustible_Estimado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Presupuesto_Combustible_Aprobado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    Creado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modificado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comisiones", x => x.ComisionId);
                    table.ForeignKey(
                        name: "FK_comisiones_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comisiones_vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "vehiculos",
                        principalColumn: "VehiculoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehiculoAccesorios",
                columns: table => new
                {
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    AccesorioId = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehiculoAccesorios", x => new { x.VehiculoId, x.AccesorioId });
                    table.ForeignKey(
                        name: "FK_vehiculoAccesorios_accesorios_AccesorioId",
                        column: x => x.AccesorioId,
                        principalTable: "accesorios",
                        principalColumn: "AccesorioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vehiculoAccesorios_vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "vehiculos",
                        principalColumn: "VehiculoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehiculoPartes",
                columns: table => new
                {
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    ParteId = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehiculoPartes", x => new { x.VehiculoId, x.ParteId });
                    table.ForeignKey(
                        name: "FK_vehiculoPartes_partes_ParteId",
                        column: x => x.ParteId,
                        principalTable: "partes",
                        principalColumn: "ParteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vehiculoPartes_vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "vehiculos",
                        principalColumn: "VehiculoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comisionDestinos",
                columns: table => new
                {
                    ComisionDestinoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kilometros = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Galones = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ComisionId = table.Column<int>(type: "int", nullable: false),
                    Creado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modificado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comisionDestinos", x => x.ComisionDestinoId);
                    table.ForeignKey(
                        name: "FK_comisionDestinos_comisiones_ComisionId",
                        column: x => x.ComisionId,
                        principalTable: "comisiones",
                        principalColumn: "ComisionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comisionUsuarios",
                columns: table => new
                {
                    ComisionId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Nombramiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Es_Piloto = table.Column<bool>(type: "bit", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Creado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modificado_Por = table.Column<int>(type: "int", nullable: true),
                    Fecha_Modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comisionUsuarios", x => new { x.ComisionId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_comisionUsuarios_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comisionUsuarios_comisiones_ComisionId",
                        column: x => x.ComisionId,
                        principalTable: "comisiones",
                        principalColumn: "ComisionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comisionViaticos",
                columns: table => new
                {
                    ComisionViaticosId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Monto_Unitario_Usado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Monto_Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ComisionId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ViaticoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comisionViaticos", x => x.ComisionViaticosId);
                    table.ForeignKey(
                        name: "FK_comisionViaticos_comisionUsuarios_ComisionId_UsuarioId",
                        columns: x => new { x.ComisionId, x.UsuarioId },
                        principalTable: "comisionUsuarios",
                        principalColumns: new[] { "ComisionId", "UsuarioId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comisionViaticos_viaticos_ViaticoId",
                        column: x => x.ViaticoId,
                        principalTable: "viaticos",
                        principalColumn: "ViaticoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "Administrador", "ADMINISTRADOR" },
                    { 2, null, "Comisionista", "COMISIONISTA" },
                    { 3, null, "Consultor", "CONSULTOR" },
                    { 4, null, "Gestor", "GESTOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permiso", "CrearComision", 1 },
                    { 2, "Permiso", "AprobarComision", 1 },
                    { 3, "Permiso", "ConsultarComision", 1 },
                    { 4, "Permiso", "EliminarComision", 1 },
                    { 5, "Permiso", "ModificarComision", 1 },
                    { 6, "Permiso", "CrearComision_Destinos", 1 },
                    { 7, "Permiso", "ModificarComision_Destinos", 1 },
                    { 8, "Permiso", "EliminarComision_Destinos", 1 },
                    { 9, "Permiso", "CrearGasolina", 1 },
                    { 10, "Permiso", "EliminarGasolina", 1 },
                    { 11, "Permiso", "ModificarGasolina", 1 },
                    { 12, "Permiso", "CrearPrecioGasolina", 1 },
                    { 13, "Permiso", "CrearViatico", 1 },
                    { 14, "Permiso", "EliminarViatico", 1 },
                    { 15, "Permiso", "ConsultarViatico", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_comisionDestinos_ComisionId",
                table: "comisionDestinos",
                column: "ComisionId");

            migrationBuilder.CreateIndex(
                name: "IX_comisiones_UsuarioId",
                table: "comisiones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_comisiones_VehiculoId",
                table: "comisiones",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_comisionUsuarios_UsuarioId",
                table: "comisionUsuarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_comisionViaticos_ComisionId_UsuarioId",
                table: "comisionViaticos",
                columns: new[] { "ComisionId", "UsuarioId" });

            migrationBuilder.CreateIndex(
                name: "IX_comisionViaticos_ViaticoId",
                table: "comisionViaticos",
                column: "ViaticoId");

            migrationBuilder.CreateIndex(
                name: "IX_gasolinaPrecios_GasolinaId",
                table: "gasolinaPrecios",
                column: "GasolinaId");

            migrationBuilder.CreateIndex(
                name: "IX_vehiculoAccesorios_AccesorioId",
                table: "vehiculoAccesorios",
                column: "AccesorioId");

            migrationBuilder.CreateIndex(
                name: "IX_vehiculoPartes_ParteId",
                table: "vehiculoPartes",
                column: "ParteId");

            migrationBuilder.CreateIndex(
                name: "IX_vehiculos_GasolinaId",
                table: "vehiculos",
                column: "GasolinaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "comisionDestinos");

            migrationBuilder.DropTable(
                name: "comisionViaticos");

            migrationBuilder.DropTable(
                name: "gasolinaPrecios");

            migrationBuilder.DropTable(
                name: "vehiculoAccesorios");

            migrationBuilder.DropTable(
                name: "vehiculoPartes");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "comisionUsuarios");

            migrationBuilder.DropTable(
                name: "viaticos");

            migrationBuilder.DropTable(
                name: "accesorios");

            migrationBuilder.DropTable(
                name: "partes");

            migrationBuilder.DropTable(
                name: "comisiones");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "vehiculos");

            migrationBuilder.DropTable(
                name: "gasolinas");
        }
    }
}
