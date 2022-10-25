using Microsoft.EntityFrameworkCore;
using MigraDoc.DocumentObjectModel;
using Shared.Data;
using Shared.Models;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Win32;
using System;
using ReportesInmobiliaria.Interfaces;
using ReportesInmobiliaria.Utilities;

namespace ReportesInmobiliaria.Services
{
    public class ReportesService : IReportesService
    {
        private readonly AppDbContext _dbContext;
        private readonly ReportesFactory _reportesFactory;
        private readonly AuxiliaryMethods _auxiliaryMethods;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly float porcentajeDescuento = 1.4066f;

        public ReportesService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory, AuxiliaryMethods auxiliaryMethods)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
            _auxiliaryMethods = auxiliaryMethods;
        }

        public async Task<byte[]> GetReporteTransaccionesCrucesTotales(string? dia, string? mes, string? semana, string? tag, string? placa, string? noEconomico)
        {
            var fechas = _auxiliaryMethods.ObtenerFechas(dia, mes, semana);

            ReporteTransacciones reporteTransaccionesTurno = new()
            {
                FechaInicio = fechas.FechaInicio,
                FechaFin = fechas.FechaFin,
                TransaccionesDetalle = await GetTransaccionesDetalleAsync(tag, placa, noEconomico, fechas)
            };

            return _reportesFactory.CrearPdf(reporteTransaccionesTurno);
        }

        public async Task<byte[]> GetReporteCrucesFerromex(string? dia, string? mes, string? semana, string? tag, string? placa, string? noEconomico)
        {
            Fechas fechas = _auxiliaryMethods.ObtenerFechas(dia, mes, semana);

            ReporteDescuentosDetalle reporteDescuentosDetalle = new()
            {
                FechaInicio = fechas.FechaInicio,
                FechaFin = fechas.FechaFin,
                DescuentosDetalle = await GetDescuentosDetalleAsync(fechas, tag, placa, noEconomico)
            };

            return _reportesFactory.CrearPdf(reporteDescuentosDetalle);

        }

        public async Task<byte[]> GetReporteCrucesFerromexResumen(string? dia, string? mes, string? semana, string? tag, string? placa, string? noEcomino)
        {
            Fechas fechas = _auxiliaryMethods.ObtenerFechas(dia, mes, semana);

            IQueryable<LaneCatalog> laneCatalogs = _dbContext.LaneCatalogs;
            var direccionEntrada = laneCatalogs.FirstOrDefault(x => x.Description == "Cuerpo A");
            var direccionSalida = laneCatalogs.FirstOrDefault(x => x.Description == "Cuerpo B");

            ReporteDescuentosResumen reporteDescuentosResumen = new()
            {
                DireccionEntrada = direccionEntrada?.Name ?? "",

                DireccionSalida = direccionSalida?.Name ?? "",

                FechaInicio = fechas.FechaInicio,

                FechaFin = fechas.FechaFin,

                DescuentosResumen = await GetDescuentosResumenAsync(fechas, tag, placa, noEcomino)
            };

            return _reportesFactory.CrearPdf(reporteDescuentosResumen);
        }

        public async Task<byte[]> GetReporteIngresosResumen(string? dia, string? mes, string? semana)
        {
            Fechas fechas = _auxiliaryMethods.ObtenerFechas(dia, mes, semana);

            ReporteIngresosResumen reporteIngresosResumen = new()
            {
                FechaInicio = fechas.FechaInicio,
                FechaFin = fechas.FechaFin,
                IngresosResumen = await GetIngresosResumenAsync(fechas)
            };
            //var res = await _dbContext.Modules.FirstOrDefaultAsync(x => x.NormalizedName == reporteMantenimiento.Tag.ToUpper());

            return _reportesFactory.CrearPdf(reporteIngresosResumen);
        }

        public async Task<byte[]> GetReporteTransaccionesTurno(string? carril, string? fecha)
        {
            Fechas fechas = _auxiliaryMethods.ObtenerFechas(fecha, null, null);

            Fechas fechaTurno = _auxiliaryMethods.ObtenerFechas(fecha, null, null);

            var res = _dbContext.Transactions
                                .Include(x => x.IdCatalogNavigation)
                                .Where(d => d.TransactionDate.Date >= fechaTurno.FechaInicio && d.TransactionDate.Date <= fechaTurno.FechaFin);

            IQueryable<LaneCatalog> laneCatalog = _dbContext.LaneCatalogs;

            var plaza = await laneCatalog.FirstOrDefaultAsync(x => res.Select(y => y.IdCatalogNavigation.IdSquare).FirstOrDefault().Equals(x.IdSquare));

            ReporteTransaccionesOperativo reporteTransaccionesDetalle = new()
            {
                Plaza = plaza?.Name ?? "",//"Atlxcayotl",
                FechaInicio = fechas.FechaInicio,
                FechaFin = fechas.FechaFin,
                TransaccionesOperativoDetalle = await GetTransaccionesDetalleTurnoAsync(carril, fechaTurno)
            };

            return _reportesFactory.CrearPdf(reporteTransaccionesDetalle);
        }

        public async Task<byte[]> GetReporteConcentradoTurno(string? carril, string? fecha)
        {
            Fechas fechas = _auxiliaryMethods.ObtenerFechas(fecha, null, null);

            var res = _dbContext.Transactions
                                .Include(x => x.IdCatalogNavigation)
                                .Where(d => d.TransactionDate.Date >= fechas.FechaInicio && d.TransactionDate.Date <= fechas.FechaFin);

            IQueryable<LaneCatalog> laneCatalog = _dbContext.LaneCatalogs;

            var plaza = await laneCatalog.FirstOrDefaultAsync(x => res.Select(y => y.IdCatalogNavigation.IdSquare).FirstOrDefault().Equals(x.IdSquare));

            ReporteOperacionesDetalle reporteTransaccionesDetalle = new()
            {
                Plaza = plaza?.Name ?? "",//"Atlxcayotl",
                FechaInicio = fechas.FechaInicio,
                FechaFin = fechas.FechaFin,
                OperacionesDetalle = await GetOperacionesDetalle(fechas, carril)
            };

            return _reportesFactory.CrearPdf(reporteTransaccionesDetalle);
        }

        public async Task<byte[]> GetReporteMantenimientoTags(string? tag, bool? estatus, string? fecha, string? noDePlaca, string? noEconomico)
        {
            var nombreUsuario = _httpContextAccessor.HttpContext?.User.FindFirstValue("NombreCompleto") ?? "Usuario no logueado";

            ReporteConcentradoTags reporteConcentradoTags = new()
            {
                Usuario = nombreUsuario ?? "",
                ConcentradoTags = GetConcentradoTags(tag, estatus, fecha, noDePlaca, noEconomico)
            };

            return _reportesFactory.CrearPdf(reporteConcentradoTags);
        }

        public async Task<byte[]> GetReportesActividadUsuarios(string? dia, string? semana, string? mes, string? nombre, string? rol, string? accion)
        {
            Fechas fecha = _auxiliaryMethods.ObtenerFechas(dia, mes, semana);

            ReporteActividadUsuarios reporteActividadUsuarios = new()
            {
                Usuario = _httpContextAccessor.HttpContext?.User.FindFirstValue("NombreCompleto") ?? "Usuario no logueado",
                FechaInicio = fecha != null ? fecha.FechaInicio : DateTime.MinValue,
                FechaFin = fecha != null ? fecha.FechaFin : DateTime.MaxValue,
                ActividadUsuario = await GetActividadUsuarios(fecha, nombre, rol, accion)
            };

            return _reportesFactory.CrearPdf(reporteActividadUsuarios);
        }



        public List<TransaccionDetalle> GetTransaccionesDetalle(DateTime fecha)
        {
            var res = _dbContext.Transactions
                .Include(x => x.IdCatalogNavigation)
                .Include(x => x.IdTariffNavigation)
                .Where(d => d.TransactionDate.Date.Equals(fecha));
            var transactions = res.ToList();

            var list = new List<TransaccionDetalle>();

            foreach (var transaction in transactions)
            {
                list.Add(new TransaccionDetalle()
                {
                    Fecha = transaction.TransactionDate,

                    Carril = transaction.IdCatalogNavigation.IdCatalog.ToString(),

                    ClasePre = transaction.IdClass.ToString(),

                    ClaseCajero = transaction.IdClass2.ToString(),

                    ClasePost = transaction.IdClass3.ToString(),

                    MedioPago = transaction.PaymentMethod,

                    Tag = transaction.IsoContent,

                    Tarifa = transaction.IdTariffNavigation.TotalTariff.ToString()

                });
            }
            return list;
        }

        public List<ConcentradoTags> GetConcentradoTags(string? tag, bool? estatus, string? fecha, string? noDePlaca, string? noEconomico)
        {

            IQueryable<TagList> res = _dbContext.TagLists;

            if (tag != null)
                res = res.Where(t => t.Tag == tag);
            if (estatus != null)
                res = res.Where(a => a.Active == estatus);
            if (fecha != null)
                res = res.Where(d => d.InsertionDate.Date == DateTime.Parse(fecha));
            if (noDePlaca != null)
                res = res.Where(a => a.VehiclePlate == noDePlaca);
            if (noEconomico != null)
                res = res.Where(a => a.EconomicNumber == noEconomico);

            var tagList = res.OrderByDescending(x => x.InsertionDate).ToList();

            var list = new List<ConcentradoTags>();

            foreach (var tags in tagList)
            {
                list.Add(new ConcentradoTags()
                {
                    Tag = tags.Tag,
                    Placa = tags.VehiclePlate,
                    NoEconomico = tags.EconomicNumber,
                    Estatus = tags.Active,
                    FechaRegistro = tags.InsertionDate.Date
                });
            }
            return list;
        }

        public async Task<List<ActividadUsuarios>> GetActividadUsuarios(Fechas fechas, string? nombre, string? rol, string? accion)
        {

            IQueryable<LogUserActivity> logUserActivities = _dbContext.LogUserActivities;
            IQueryable<LogRole> logRoles = _dbContext.LogRoles;
            IQueryable<LogTagList> logTagLists = _dbContext.LogTagLists;
            IQueryable<LogRoleModule> logRoleModules = _dbContext.LogRoleModules;
            IQueryable<AspNetUser> users = _dbContext.AspNetUsers.Include(x => x.AspNetUserRoles);
            IQueryable<ActionCatalog> actions = _dbContext.ActionCatalogs;
            IQueryable<AspNetRole> roles = _dbContext.AspNetRoles;
            IQueryable<Shared.Models.Module> modules = _dbContext.Modules;
            IQueryable<AspNetUserRole> aspNetUserRoles = _dbContext.AspNetUserRoles;
            var list = new List<ActividadUsuarios>();

            //Llenado de lista desde logUsersActivities
            var logUserActivitiesTotales = logUserActivities;

            if (fechas != null)
                logUserActivities = logUserActivities.Where(d => d.UpdatedDate.Date >= fechas.FechaInicio && d.UpdatedDate.Date <= fechas.FechaFin);

            if (nombre != null)
            {
                string[] nombres = nombre.Split(" ");

                if (nombres.Length > 1)
                {
                    foreach (var nom in nombres)
                    {
                        logUserActivities = logUserActivities
                            .Where(x => users.Where(y => y.Name == nom).Select(z => z.Id).Contains(x.IdUpdatedUser)
                            || users.Where(y => y.LastName == nom).Select(z => z.Id).Contains(x.IdUpdatedUser));
                    }
                }
                else
                    logUserActivities = logUserActivities
                        .Where(x => users.Where(y => y.Name == nombre).Select(z => z.Id).Contains(x.IdUpdatedUser)
                        || users.Where(y => y.LastName == nombre).Select(z => z.Id).Contains(x.IdUpdatedUser));
            }

            if (rol != null)
                logUserActivities = logUserActivities.Where(x => x.IdModifiedUser == aspNetUserRoles.FirstOrDefault(y => y.RoleId == roles.FirstOrDefault(z => z.Name == rol).Id).UserId);

            if (accion != null)
                logUserActivities = logUserActivities.Where(x => x.TypeAction == actions.FirstOrDefault(y => y.IdActionCatalog == accion).IdActionCatalog);

            foreach (var actividad in logUserActivities)
            {
                string registroOriginal = "";
                string registroEditado = "";

                var userName = users.FirstOrDefault(x => x.Id == actividad.IdModifiedUser)?.Name ?? "";
                var userLastName = users.FirstOrDefault(x => x.Id == actividad.IdModifiedUser)?.LastName ?? "";

                switch (actions.FirstOrDefault(x => x.IdActionCatalog == actividad.TypeAction).TypeAction)
                {
                    case "Actualizar Rol":
                        registroOriginal = roles?.FirstOrDefault(x => x.Id == actividad.AspNetRolesIdOld)?.Name ?? "";
                        registroEditado = roles?.FirstOrDefault(x => x.Id == actividad.AspNetRolesIdNew)?.Name ?? "";
                        break;
                    case "Crear Usuario":
                        //registroOriginal = roles?.FirstOrDefault(x => x.Id == actividad.AspNetRolesIdOld)?.Name;
                        registroEditado = actividad.NewName + " " + actividad.NewLastName;
                        break;
                    case "Actualizar Estatus de Usuario":
                        if (logUserActivitiesTotales.Where(x => x.UpdatedDate <= actividad.UpdatedDate && x.IdUpdatedUser == actividad.IdUpdatedUser).Count() > 1 && actividad.Active == true)
                        {
                            registroOriginal = userName + " " + userLastName + " (Inactivo)";
                            registroEditado = userName + " " + userLastName + " (Activo)";
                        }
                        else
                        {
                            registroOriginal = userName + " " + userLastName + " (Activo)";
                            registroEditado = userName + " " + userLastName + " (Inactivo)";
                        }
                        break;
                    case "Actualizar Nombre de Usuario":
                        registroOriginal = actividad.OldName + " " + actividad.OldLastName;
                        registroEditado = actividad.NewName + " " + actividad.NewLastName;
                        break;
                    case "Actualizar Contraseña":
                        registroOriginal = actividad.OldPass ?? "";
                        registroEditado = actividad.NewePass ?? "";
                        break;
                    default:
                        break;
                }
                list.Add(new ActividadUsuarios()
                {
                    Nombre = userName + " " + userLastName,
                    Rol = roles.FirstOrDefault(x => x.Id == aspNetUserRoles.FirstOrDefault(y => y.UserId == actividad.IdModifiedUser).RoleId)?.Name ?? "",
                    FechaMovimiento = actividad.UpdatedDate,
                    Modulo = modules.FirstOrDefault(x => actions.FirstOrDefault(y => actividad.TypeAction == y.IdActionCatalog).IdModule == x.Id)?.NameModule ?? "",
                    Accion = actions.FirstOrDefault(x => x.IdActionCatalog == actividad.TypeAction)?.TypeAction ?? "",
                    RegistroOriginal = registroOriginal,
                    RegistroEditado = registroEditado
                });
            }

            //Llenado de lista desde logRoles

            var logRolesTotales = logRoles;

            if (fechas != null)
                logRoles = logRoles.Where(d => d.UpdatedDate.Date >= fechas.FechaInicio && d.UpdatedDate.Date <= fechas.FechaFin);

            if (rol != null)
                logRoles = logRoles.Where(x => x.IdUser == aspNetUserRoles.FirstOrDefault(y => y.RoleId == roles.FirstOrDefault(z => z.Name == rol).Id).UserId);
            if (accion != null)
                logRoles = logRoles.Where(x => x.TypeAction == actions.FirstOrDefault(y => y.IdActionCatalog == accion).IdActionCatalog);

            if (nombre != null)
            {
                string[] nombres = nombre.Split(" ");

                if (nombres.Length > 1)
                {
                    foreach (var nom in nombres)
                    {
                        logRoles = logRoles
                            .Where(x => users.Where(y => y.Name == nom).Select(z => z.Id).Contains(x.IdUser)
                            || users.Where(y => y.LastName == nom).Select(z => z.Id).Contains(x.IdUser));
                    }
                }
                else
                    logRoles = logRoles
                        .Where(x => users.Where(y => y.Name == nombre).Select(z => z.Id).Contains(x.IdUser)
                        || users.Where(y => y.LastName == nombre).Select(z => z.Id).Contains(x.IdUser));
            }

            foreach (var actividad in logRoles)
            {
                var userName = users.FirstOrDefault(x => x.Id == actividad.IdUser)?.Name ?? "";
                var userLastName = users.FirstOrDefault(x => x.Id == actividad.IdUser)?.LastName ?? "";

                var registroEditado = "";
                var registroOriginal = "";

                if (logRolesTotales.Where(x => x.UpdatedDate <= actividad.UpdatedDate && x.AspNetRolesId == actividad.AspNetRolesId).Count() > 1 && actividad.Active == true)
                {
                    registroOriginal = actividad.OldNameRol + " (Inactivo)";
                    registroEditado = actividad.NewNameRol + " (Activo)";
                }
                else if (actividad.Active == true)
                {
                    registroEditado = actividad.NewNameRol;
                }
                else
                {
                    registroOriginal = actividad.OldNameRol + " (Activo)";
                    registroEditado = actividad.NewNameRol + " (Inactivo)";
                }
                list.Add(new ActividadUsuarios()
                {
                    Nombre = userName + " " + userLastName,
                    Rol = roles.FirstOrDefault(x => x.Id == aspNetUserRoles.FirstOrDefault(y => y.UserId == actividad.IdUser).RoleId)?.Name ?? "",
                    FechaMovimiento = actividad.UpdatedDate,
                    Modulo = modules.FirstOrDefault(x => actions.FirstOrDefault(y => actividad.TypeAction == y.IdActionCatalog).IdModule == x.Id)?.NameModule ?? "",
                    Accion = actions.FirstOrDefault(x => x.IdActionCatalog == actividad.TypeAction)?.TypeAction ?? "",
                    RegistroOriginal = registroOriginal,
                    RegistroEditado = registroEditado
                });

            }

            //Llenado de lista desde logRoleModule

            var logRoleModulesTotales = logRoleModules;

            if (fechas != null)
                logRoleModules = logRoleModules.Where(d => d.UpdatedDate.Date >= fechas.FechaInicio && d.UpdatedDate.Date <= fechas.FechaFin);

            if (nombre != null)
            {
                string[] nombres = nombre.Split(" ");

                if (nombres.Length > 1)
                {
                    foreach (var nom in nombres)
                    {
                        logRoleModules = logRoleModules
                            .Where(x => users.Where(y => y.Name == nom).Select(z => z.Id).Contains(x.IdUser)
                            || users.Where(y => y.LastName == nom).Select(z => z.Id).Contains(x.IdUser));
                    }
                }
                else
                    logRoleModules = logRoleModules
                        .Where(x => users.Where(y => y.Name == nombre).Select(z => z.Id).Contains(x.IdUser)
                        || users.Where(y => y.LastName == nombre).Select(z => z.Id).Contains(x.IdUser));
            }

            if (rol != null)
                logRoleModules = logRoleModules.Where(x => x.IdUser == aspNetUserRoles.FirstOrDefault(y => y.RoleId == roles.FirstOrDefault(z => z.Name == rol).Id).UserId);
            if (accion != null)
                logRoleModules = logRoleModules.Where(x => x.TypeAction == actions.FirstOrDefault(y => y.IdActionCatalog == accion).IdActionCatalog);

            foreach (var actividad in logRoleModules)
            {
                var userName = users.FirstOrDefault(x => x.Id == actividad.IdUser)?.Name ?? "";
                var userLastName = users.FirstOrDefault(x => x.Id == actividad.IdUser)?.LastName ?? "";

                list.Add(new ActividadUsuarios()
                {
                    Nombre = userName + " " + userLastName,
                    Rol = roles.FirstOrDefault(x => x.Id == aspNetUserRoles.FirstOrDefault(y => y.UserId == actividad.IdUser).RoleId)?.Name ?? "",
                    FechaMovimiento = actividad.UpdatedDate,
                    Modulo = modules.FirstOrDefault(x => x.Id == actions.FirstOrDefault(y => y.IdActionCatalog == actividad.TypeAction).IdModule)?.NameModule ?? "",
                    Accion = actions.FirstOrDefault(x => x.IdActionCatalog == actividad.TypeAction)?.TypeAction ?? "",
                    RegistroOriginal = roles?.FirstOrDefault(x => actividad.AspNetRolesId == x.Id)?.Name ?? "",
                    RegistroEditado = modules.FirstOrDefault(x => actividad.ModulesId == x.Id)?.NameModule ?? "",
                });

            }

            //Llenado desde logTagList

            var logTagListTotales = logTagLists;

            if (fechas != null)
                logTagLists = logTagLists.Where(d => d.UpdatedDate.Date >= fechas.FechaInicio && d.UpdatedDate.Date <= fechas.FechaFin);

            if (nombre != null)
            {
                string[] nombres = nombre.Split(" ");

                if (nombres.Length > 1)
                {
                    foreach (var nom in nombres)
                    {
                        logTagLists = logTagLists
                            .Where(x => users.Where(y => y.Name == nom).Select(z => z.Id).Contains(x.IdUser)
                            || users.Where(y => y.LastName == nom).Select(z => z.Id).Contains(x.IdUser));
                    }
                }
                else
                    logTagLists = logTagLists
                        .Where(x => users.Where(y => y.Name == nombre).Select(z => z.Id).Contains(x.IdUser)
                        || users.Where(y => y.LastName == nombre).Select(z => z.Id).Contains(x.IdUser));
            }

            if (rol != null)
                logTagLists = logTagLists.Where(x => x.IdUser == aspNetUserRoles.FirstOrDefault(y => y.RoleId == roles.FirstOrDefault(z => z.Name == rol).Id).UserId);

            foreach (var actividad in logTagLists)
            {
                var userName = users.FirstOrDefault(x => x.Id == actividad.IdUser)?.Name ?? "";
                var userLastName = users.FirstOrDefault(x => x.Id == actividad.IdUser)?.LastName ?? "";

                var registroEditado = "";
                var registroOriginal = "";
                var action = "";

                if (logTagListTotales.Where(x => x.UpdatedDate <= actividad.UpdatedDate && x.Tag == actividad.Tag).Count() > 1 && actividad.Active == true)
                {
                    registroOriginal = actividad.Tag + " (Inactivo)";
                    registroEditado = actividad.Tag + " (Activo)";
                    action = "Actualizar Estatus de Tag";
                }
                else if (actividad.Active == true)
                {
                    registroEditado = actividad.Tag;
                    action = "Añadir Tag";
                }
                else
                {
                    registroOriginal = actividad.Tag + " (Activo)";
                    registroEditado = actividad.Tag + " (Inactivo)";
                    action = "Actualizar Estatus de Tag";
                }

                if (accion == null || actions.FirstOrDefault(x => x.IdActionCatalog == accion).TypeAction == action)
                {
                    list.Add(new ActividadUsuarios()
                    {
                        Nombre = userName + " " + userLastName,
                        Rol = roles.FirstOrDefault(x => x.Id == aspNetUserRoles.FirstOrDefault(y => y.UserId == actividad.IdUser).RoleId)?.Name ?? "",
                        FechaMovimiento = actividad.UpdatedDate,
                        Modulo = "Gestión de Tags",
                        Accion = action,
                        RegistroOriginal = registroOriginal,
                        RegistroEditado = registroEditado
                    });
                }
            }

            return list.OrderBy(x => x.FechaMovimiento).ToList();
        }
        async Task<List<DescuentoDetalle>> GetDescuentosDetalleAsync(Fechas fechas, string? tag, string? placa, string? noEconomico)
        {
            IQueryable<TagList> tags = _dbContext.TagLists.Where(x => x.Active);
            var lanes = _dbContext.LaneCatalogs.ToList();

            if (placa != null)
                tags = tags.Where(x => x.VehiclePlate == placa);

            if (noEconomico != null)
                tags = tags.Where(x => x.EconomicNumber == noEconomico);

            if (tag != null)
                tags = tags.Where(x => tag.Equals(x.Tag.Trim()));

            var res = _dbContext.Transactions
                .Include(x => x.IdCatalogNavigation)
                .Include(x => x.IdClass2Navigation)
                .Include(x => x.IdTariffNavigation)
                .Where(d => d.TransactionDate.Date >= fechas.FechaInicio &&
                            d.TransactionDate.Date <= fechas.FechaFin &&
                            tags.Select(y => y.Tag).Contains(d.IsoContent.Trim()))
                .OrderBy(x => x.TransactionDate);

            var transactions = await res.ToListAsync();
            var tagsList = await tags.ToListAsync();

            var list = new List<DescuentoDetalle>();

            foreach (var transaction in transactions)
            {
                var TagInfo = tagsList.FirstOrDefault(x => x.Tag.Equals(transaction.IsoContent.Trim()));
                list.Add(new DescuentoDetalle()
                {
                    Fecha = transaction.TransactionDate,
                    Carril = transaction.IdCatalogNavigation?.IdLane ?? "",
                    Clase = transaction.IdClass2Navigation?.ClassCode ?? "",
                    Descuento = transaction.IdDiscount == 3,
                    Tag = transaction.IsoContent?.Trim() ?? "",
                    Placa = TagInfo?.VehiclePlate ?? "",
                    NoEconomico = TagInfo?.EconomicNumber ?? "",
                    Tarifa = transaction.IdTariffNavigation?.Tariff1.ToString("C") ?? "",
                    TarifaDesc = transaction.IdDiscount == 3 ? ((int)transaction.IdTariffNavigation?.Tariff1 / porcentajeDescuento).ToString("C") ?? "" : "N/A",
                    Cuerpo = transaction.IdCatalogNavigation?.IdSide != null ? lanes.FirstOrDefault(x => x.IdSide == transaction.IdCatalogNavigation.IdSide)?.Name : null
                });
            }
            return list;
        }

        async Task<List<IngresoResumen>> GetIngresosResumenAsync(Fechas? fechas)
        {
            var transactionsDb = _dbContext.Transactions.Include(x => x.IdTariffNavigation);

            var tags = _dbContext.TagLists.Where(x => x.Active);

            var list = new List<IngresoResumen>();

            var transactions = transactionsDb.Where(x => tags.Select(y => y.Tag).Contains(x.IsoContent.Trim()));

            var transactionsList = transactions
                .Where(d => d.TransactionDate.Date >= fechas.FechaInicio && d.TransactionDate.Date <= fechas.FechaFin)
                .ToList();

            list.Add(new IngresoResumen()
            {
                MedioPago = "Transacciones Intermodal",
                Cantidad = transactionsList.Count.ToString(),
                Ingreso = transactionsList.Sum(x => x.IdTariffNavigation.Tariff1).ToString("C")
            });
            list.Add(new IngresoResumen()
            {
                MedioPago = "Tarifa Base",
                Cantidad = transactionsList.Where(x => x.IdDiscount != 3).Count().ToString(),
                Ingreso = transactionsList.Where(x => x.IdDiscount != 3).Sum(x => x.IdTariffNavigation.Tariff1).ToString("C")
            });
            list.Add(new IngresoResumen()
            {
                MedioPago = "Tarifa Descuento",
                Cantidad = transactionsList.Where(x => x.IdDiscount == 3).Count().ToString(),
                Ingreso = transactionsList.Where(x => x.IdDiscount == 3).Sum(x => x.IdTariffNavigation.Tariff1).ToString("C")
            });

            return list;
        }

        async Task<List<TransaccionDetalle>> GetTransaccionesDetalleAsync(string? tag, string? placa, string? noEconomico, Fechas fechas = null, int? bolsa = null)
        {
            IQueryable<TagList> tags = _dbContext.TagLists.Where(x => x.Active);

            var lanes = _dbContext.LaneCatalogs.ToList();

            IQueryable<Transaction> res = _dbContext.Transactions
                .Include(x => x.IdCatalogNavigation)
                .Include(x => x.IdClass2Navigation)
                .Include(x => x.IdTariffNavigation)
                .Include(x => x.IdPaymentNavigation)
                .OrderBy(x => x.TransactionDate);

            if (tag != null)
                res = res.Where(x => tag.Equals(x.IsoContent.Trim()));
            if (noEconomico != null)
                tags = tags.Where(x => x.EconomicNumber == noEconomico);
            if (placa != null)
                tags = tags.Where(x => x.VehiclePlate == placa);

            if (bolsa != null)
                res = res.Where(x => x.IdSac == bolsa);
            if (fechas != null)
                res = res.Where(d => d.TransactionDate.Date >= fechas.FechaInicio && d.TransactionDate.Date <= fechas.FechaFin);

            var tagsList = await tags.ToListAsync();
            if (placa != null || noEconomico != null)
                res = res.Where(x => tagsList.Select(y => y.Tag).Contains(x.IsoContent.Trim()));


            var transactions = await res.ToListAsync();
            var classes = await _dbContext.TypeClasses.ToListAsync();
            var list = new List<TransaccionDetalle>();

            var listA = new List<TransaccionDetalle>();

            foreach (var transaction in transactions)
            {
                var TagInfo = tagsList.FirstOrDefault(x => x.Tag.Equals(transaction.IsoContent.Trim()));
                list.Add(new TransaccionDetalle()
                {
                    Fecha = transaction.TransactionDate,
                    Carril = transaction.IdCatalogNavigation?.IdLane ?? "",
                    Tag = transaction.IsoContent?.Trim() ?? "",
                    Placa = TagInfo?.VehiclePlate ?? "N/A",
                    NoEconomico = TagInfo?.EconomicNumber ?? "N/A",
                    Tarifa = transaction.IdTariffNavigation?.Tariff1.ToString("C") ?? "",
                    TarifaDescuento = transaction.IdDiscount == 3 ? ((int)transaction.IdTariffNavigation?.Tariff1 / porcentajeDescuento).ToString("C") ?? "" : "N/A",
                    ClasePre = transaction.IdClass2Navigation?.ClassCode ?? "",
                    ClaseCajero = classes.FirstOrDefault(x => x.IdClass == transaction.IdClass2)?.ClassCode ?? "",
                    ClasePost = classes.FirstOrDefault(x => x.IdClass == transaction.IdClass3)?.ClassCode ?? "",
                    MedioPago = transaction.IdPaymentNavigation?.PaymentName ?? "",
                    Cuerpo = transaction.IdCatalogNavigation?.IdSide != null ? lanes.FirstOrDefault(x => x.IdSide == transaction.IdCatalogNavigation.IdSide)?.Name : null
                });
            }
            return list;
        }

        async Task<List<TransaccionOperativoDetalle>> GetTransaccionesDetalleTurnoAsync(string? carril, Fechas fecha)
        {
            IQueryable<LaneCatalog> laneCatalog = _dbContext.LaneCatalogs;
            var lanes = _dbContext.LaneCatalogs.ToList();

            var res = _dbContext.Transactions
                .Include(x => x.IdCatalogNavigation)
                .Include(x => x.IdClass2Navigation)
                .Include(x => x.IdTariffNavigation)
                .Include(x => x.IdPaymentNavigation)
                .Where(d => d.TransactionDate.Date >= fecha.FechaInicio && d.TransactionDate.Date <= fecha.FechaFin);

            if (carril != null)
                res = res.Where(x => x.IdCatalogNavigation.IdLane.Equals(carril));

            var transactions = await res.ToListAsync();
            var classes = await _dbContext.TypeClasses.ToListAsync();
            var list = new List<TransaccionOperativoDetalle>();

            foreach (var transaction in transactions)
            {
                list.Add(new TransaccionOperativoDetalle()
                {
                    Fecha = transaction.TransactionDate,
                    Carril = transaction.IdCatalogNavigation?.IdLane ?? "",
                    Tag = transaction.IsoContent?.Trim() ?? "",
                    Tarifa = transaction.IdTariffNavigation?.Tariff1.ToString("C") ?? "",
                    ClasePre = transaction.IdClass2Navigation?.ClassCode ?? "",
                    ClaseCajero = classes.FirstOrDefault(x => x.IdClass == transaction.IdClass2)?.ClassCode ?? "",
                    ClasePost = classes.FirstOrDefault(x => x.IdClass == transaction.IdClass3)?.ClassCode ?? "",
                    MedioPago = transaction.IdPaymentNavigation?.PaymentName ?? "",
                    Cuerpo = transaction.IdCatalogNavigation?.IdSide != null ? lanes.FirstOrDefault(x => x.IdSide == transaction.IdCatalogNavigation.IdSide)?.Name : null
                });
            }
            return list;
        }

        async Task<List<DescuentoResumen>> GetDescuentosResumenAsync(Fechas? fechas, string? tagB, string? placa, string? noEconomico)
        {
            IQueryable<TagList> tagsFerromex = _dbContext.TagLists.Where(x => x.Active);

            if (placa != null)
                tagsFerromex = tagsFerromex.Where(x => x.VehiclePlate == placa);

            if (noEconomico != null)
                tagsFerromex = tagsFerromex.Where(x => x.EconomicNumber == noEconomico);

            if (tagB != null)
                tagsFerromex = tagsFerromex.Where(x => tagB.Equals(x.Tag.Trim()));

            IQueryable<Transaction> res = _dbContext.Transactions
                .Include(x => x.IdCatalogNavigation)
                .Include(x => x.IdClass2Navigation)
                .Include(x => x.IdTariffNavigation)
                .Include(x => x.IdPaymentNavigation)
                .Where(x => tagsFerromex.Select(y => y.Tag).Contains(x.IsoContent.Trim()) && x.TransactionDate.Date >= fechas.FechaInicio && x.TransactionDate.Date <= fechas.FechaFin)
                .OrderBy(x => x.TransactionDate);

            var sideA = await _dbContext.LaneCatalogs.Where(x => x.Description.Equals("Cuerpo A")).ToListAsync();
            var sideB = await _dbContext.LaneCatalogs.Where(x => x.Description.Equals("Cuerpo B")).ToListAsync();
            var tagList = await tagsFerromex.ToListAsync();
            var transactions = await res.ToListAsync();

            var tags = transactions.Select(x => x.IsoContent).Distinct().ToList();

            IQueryable<LaneCatalog> laneCatalogs = _dbContext.LaneCatalogs;

            var idSidesCuerpos = laneCatalogs.Select(x => new { x.IdSide, x.Description })
                                      .Where(x => !string.IsNullOrWhiteSpace(x.IdSide) && (x.Description == "Cuerpo A" || x.Description == "Cuerpo B"))
                                      .ToList()
                                      .GroupBy(x => x.Description);

            IEnumerable<Transaction> crucesEntradaTotales = transactions;
            IEnumerable<Transaction> crucesSalidaTotales = transactions;
            foreach (var item in idSidesCuerpos)
            {
                if (item.Key == "Cuerpo A") crucesEntradaTotales = transactions.Where(x => item.Any(y => y.IdSide == x.IdCatalogNavigation.IdSide));
                if (item.Key == "Cuerpo B") crucesSalidaTotales = transactions.Where(x => item.Any(y => y.IdSide == x.IdCatalogNavigation.IdSide));
            }

            var list = new List<DescuentoResumen>();
            foreach (var tag in tags)
            {
                if (string.IsNullOrWhiteSpace(tag.Trim())) continue;

                var crucesEntrada = crucesEntradaTotales.Where(x => x.IsoContent.Contains(tag.Trim())).OrderByDescending(x => x.TransactionDate);
                var crucesSalida = crucesSalidaTotales.Where(x => x.IsoContent.Contains(tag.Trim())).OrderByDescending(x => x.TransactionDate).ToArray();

                foreach (var (cruceEntrada, index) in crucesEntrada.Select((v, i) => (v, i)))
                {
                    var TagInfo = tagList.FirstOrDefault(x => x.Tag.Equals(tag.Trim()));
                    var obj = new DescuentoResumen
                    {
                        FechaEntrada = cruceEntrada?.TransactionDate ?? DateTime.MinValue,
                        CarrilEntrada = cruceEntrada?.IdCatalogNavigation?.IdLane ?? "",
                        ClaseEntrada = cruceEntrada?.IdClass2Navigation?.ClassCode ?? "",
                        TarifaEntrada = cruceEntrada?.IdTariffNavigation?.Tariff1.ToString("C") ?? "",

                        FechaSalida = index < crucesSalida.Length ? cruceEntrada?.TransactionDate > crucesSalida[index]?.TransactionDate ? null : crucesSalida[index]?.TransactionDate : null,

                        //FechaSalida = index < crucesSalida.Length ? crucesSalida[index]?.TransactionDate ?? null : null,
                        CarrilSalida = index < crucesSalida.Length ? crucesSalida[index]?.IdCatalogNavigation?.IdLane ?? "" : "",
                        ClaseSalida = index < crucesSalida.Length ? crucesSalida[index]?.IdClass2Navigation?.ClassCode ?? "" : "",
                        TarifaSalida = index < crucesSalida.Length ? crucesSalida[index]?.IdTariffNavigation?.Tariff1.ToString("C") ?? "" : "",
                        TarifaDescuento = index < crucesSalida.Length ? crucesSalida[index]?.IdDiscount == 3 ? ((int)crucesSalida[index]?.IdTariffNavigation?.Tariff1 / porcentajeDescuento).ToString("C") ?? "" : "N/A" : "",
                        Descuento = index < crucesSalida.Length && crucesSalida[index]?.IdDiscount == 3,

                        Tag = tag,
                        Placa = TagInfo?.VehiclePlate ?? "",
                        NoEconomico = TagInfo?.EconomicNumber ?? ""
                    };
                    if (obj.FechaSalida == null)
                    {
                        obj.CarrilSalida = "";
                        obj.ClaseSalida = "";
                        obj.TarifaSalida = "";
                        obj.TarifaDescuento = "";
                    }
                    list.Add(obj);
                }
            }
            return list;
        }

        async Task<List<OperacionDetalle>> GetOperacionesDetalle(Fechas fechas, string? carril)
        {
            var list = new List<OperacionDetalle>();
            Dictionary<int, string> collection = new()
            {
                { 1, "Efectivo" },
                { 15, "Prepago TAG" },
                { 0, "Tag Intermodal Tar Base" },
                { 3, "Tag Intermodal Tar Desc" }
            };

            IQueryable<TagList> tagList = _dbContext.TagLists;

            foreach (var item in collection)
            {
                var res = _dbContext.Transactions
                    .Include(x => x.IdClass2Navigation)
                    .Include(x => x.IdTariffNavigation)
                    .Include(x => x.IdPaymentNavigation)
                    .Where(d => d.TransactionDate.Date >= fechas.FechaInicio && d.TransactionDate.Date <= fechas.FechaFin);

                if (carril != null)
                    res = res.Where(x => x.IdCatalogNavigation.IdLane == carril);

                if (item.Value == "Efectivo" || item.Value == "Prepago TAG") res = res.Where(x => x.IdPaymentNavigation.IdPayment == item.Key && !tagList.Select(y => y.Tag).Contains(x.IsoContent.Trim()));
                if (item.Value == "Tag Intermodal Tar Base")
                    res = res.Where(x => x.IdDiscount != 3 && tagList.Select(y => y.Tag).Contains(x.IsoContent.Trim()));
                if (item.Value == "Tag Intermodal Tar Desc")
                    res = res.Where(x => x.IdDiscount == 3);

                OperacionDetalle operaciones = new OperacionDetalle();
                PropertyInfo[] properties = typeof(OperacionDetalle).GetProperties();

                Dictionary<string, string> classCode = new()
                {
                    { "T01a","A" },
                    { "T01m","M" },
                    { "T02b","B2" },
                    { "T02c","C2" },
                    { "T03b","B3" },
                    { "T03c","C3" },
                    { "T04c","C4" },
                    { "T05c","C5" },
                    { "T06c","C6" },
                    { "T07c","C7" },
                    { "T08c","C8" },
                    { "T09c","C9" }
                };

                foreach (PropertyInfo property in properties)
                {
                    if (property.Name == "MedioPago")
                        property.SetValue(operaciones, item.Value);
                    else if (property.Name == "CantidadTotal")
                    {
                        var aux = 0;
                        foreach (var code in classCode)
                        {
                            aux += res.Where(x => x.IdClass2Navigation.ClassCode == code.Value).Count();
                        }
                        property.SetValue(operaciones, aux.ToString());
                    }
                    else if (property.Name == "IngresoTotal")
                    {
                        decimal aux = 0;
                        foreach (var code in classCode)
                        {
                            aux += res.Where(x => x.IdClass2Navigation.ClassCode == code.Value).Sum(x => x.IdTariffNavigation.Tariff1);
                        }
                        property.SetValue(operaciones, aux.ToString());
                    }
                    else
                    {
                        foreach (var code in classCode)
                        {
                            if (property.Name == "Cantidad" + code.Key)
                            {
                                property.SetValue(operaciones, res.Where(x => x.IdClass2Navigation.ClassCode == code.Value).Count().ToString());
                            }
                            else if (property.Name == "Ingreso" + code.Key)
                            {
                                property.SetValue(operaciones, res.Where(x => x.IdClass2Navigation.ClassCode == code.Value).Sum(x => x.IdTariffNavigation.Tariff1).ToString("C"));
                            }
                        }
                    }
                }
                list.Add(operaciones);
            }
            list.Add(
                new OperacionDetalle()
                {
                    MedioPago = "Total",
                    CantidadT01a = list.Sum(x => decimal.TryParse(x.CantidadT01a, out decimal sum) ? sum : 0).ToString(),
                    CantidadT01m = list.Sum(x => decimal.TryParse(x.CantidadT01m, out decimal sum) ? sum : 0).ToString(),
                    CantidadT02b = list.Sum(x => decimal.TryParse(x.CantidadT02b, out decimal sum) ? sum : 0).ToString(),
                    CantidadT02c = list.Sum(x => decimal.TryParse(x.CantidadT02c, out decimal sum) ? sum : 0).ToString(),
                    CantidadT03b = list.Sum(x => decimal.TryParse(x.CantidadT03b, out decimal sum) ? sum : 0).ToString(),
                    CantidadT03c = list.Sum(x => decimal.TryParse(x.CantidadT03c, out decimal sum) ? sum : 0).ToString(),
                    CantidadT04c = list.Sum(x => decimal.TryParse(x.CantidadT04c, out decimal sum) ? sum : 0).ToString(),
                    CantidadT05c = list.Sum(x => decimal.TryParse(x.CantidadT05c, out decimal sum) ? sum : 0).ToString(),
                    CantidadT06c = list.Sum(x => decimal.TryParse(x.CantidadT06c, out decimal sum) ? sum : 0).ToString(),
                    CantidadT07c = list.Sum(x => decimal.TryParse(x.CantidadT07c, out decimal sum) ? sum : 0).ToString(),
                    CantidadT08c = list.Sum(x => decimal.TryParse(x.CantidadT08c, out decimal sum) ? sum : 0).ToString(),
                    CantidadT09c = list.Sum(x => decimal.TryParse(x.CantidadT09c, out decimal sum) ? sum : 0).ToString(),
                    CantidadTotal = list.Sum(x => decimal.TryParse(x.CantidadTotal, out decimal sum) ? sum : 0).ToString(),
                    IngresoT01a = list.Sum(x => decimal.TryParse(x.IngresoT01a, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoT01m = list.Sum(x => decimal.TryParse(x.IngresoT01m, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoT02b = list.Sum(x => decimal.TryParse(x.IngresoT02b, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoT02c = list.Sum(x => decimal.TryParse(x.IngresoT02c, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoT03b = list.Sum(x => decimal.TryParse(x.IngresoT03b, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoT03c = list.Sum(x => decimal.TryParse(x.IngresoT03c, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoT04c = list.Sum(x => decimal.TryParse(x.IngresoT04c, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoT05c = list.Sum(x => decimal.TryParse(x.IngresoT05c, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoT06c = list.Sum(x => decimal.TryParse(x.IngresoT06c, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoT07c = list.Sum(x => decimal.TryParse(x.IngresoT07c, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoT08c = list.Sum(x => decimal.TryParse(x.IngresoT08c, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoT09c = list.Sum(x => decimal.TryParse(x.IngresoT09c, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                    IngresoTotal = list.Sum(x => decimal.TryParse(x.IngresoTotal, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sum) ? sum : 0).ToString("C"),
                }
                );

            return list;
        }


    }

}

