using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ReportesData.Models;
using TestingFrontEnd.Interfaces;
using TestingFrontEnd.Stores;

namespace TestingFrontEnd.Pages.Reportes
{
    public partial class DiaCasetaIndex : ComponentBase
    {
        private readonly IReportesService _reportesService;
        private ApplicationContext _context;
        public EditContext EditContext;
        private TurnoCarriles ReporteTurnoCarriles { get; set; }

        private TypePlaza Plaza { get; set; }
        private TypeDelegacion Delegacion { get; set; }
        private UsuarioPlaza UsuarioPlaza { get; set; }
        private List<Personal> Administradores { get; set; }
        private List<Personal> EncargadosTurno { get; set; }

        private KeyValuePair<string, string>[] Turnos;
        private byte[]? PdfBlob { get; set; }
        private bool Render;
        private bool HideLoader = true;
        private bool HideError = true;

        public DiaCasetaIndex(IReportesService reportesService, ApplicationContext context)
        {
            _reportesService = reportesService;
            _context = context;
        }

        protected override async Task OnInitializedAsync()
        {
            ReporteTurnoCarriles = new() { Fecha = DateTime.Now };
            EditContext = new EditContext(ReporteTurnoCarriles);

            UsuarioPlaza = await _reportesService.GetUsuarioPlazaAsync();
            ReporteTurnoCarriles.NumPlaza = UsuarioPlaza?.NumPlaza;
            ReporteTurnoCarriles.NumDelegacion = UsuarioPlaza?.NumDelegacion;

            if (UsuarioPlaza != null)
            {
                var plazas = await _reportesService.GetPlazasAsync();
                Plaza = plazas.FirstOrDefault(x => x.NumPlaza == UsuarioPlaza.NumPlaza);

                var delegaciones = await _reportesService.GetDelegacionesAsync();
                Delegacion = delegaciones.FirstOrDefault(x => x.NumDelegacion == UsuarioPlaza.NumDelegacion);

                Turnos = await _reportesService.GetTurnosAsync();
                ReporteTurnoCarriles.IdTurno = Turnos?.FirstOrDefault().Key;

                EncargadosTurno = await _reportesService.GetEncargadosTurnoAsync();
                ReporteTurnoCarriles.NumGeaEncargadoTurno = EncargadosTurno?.FirstOrDefault().NumGea;

                Administradores = await _reportesService.GetAdministradoresAsync();
                ReporteTurnoCarriles.NumGeaAdministrador = Administradores?.FirstOrDefault().NumGea;

                Render = true;
            }
        }

        private async void GenerarReporte()
        {
            HideLoader = false;

            if (EditContext.Validate())
            {
                PdfBlob = await _reportesService.CreateReporteTurnoCarrilesAsync(ReporteTurnoCarriles);
            }
            else
            {
                Console.WriteLine("Form is Invalid");
            }

            HideLoader = true;
        }
    }
}
