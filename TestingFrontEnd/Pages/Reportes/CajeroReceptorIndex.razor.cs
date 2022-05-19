using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ReportesData.Models;
using Microsoft.Fast.Components.FluentUI;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TestingFrontEnd.Models;
using TestingFrontEnd.Stores;
using TestingFrontEnd.Services;
using TestingFrontEnd.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace TestingFrontEnd.Pages.Reportes
{
    public partial class CajeroReceptorIndex : ComponentBase
    {
        private readonly IReportesService _reportesService;
        private readonly IJSInProcessRuntime _js;
        public EditContext EditContext;
        private CajeroReceptor ReporteCajeroReceptorModel { get; set; }

        private TypePlaza Plaza = new();
        private TypeDelegacion Delegacion = new();
        private UsuarioPlaza UsuarioPlaza = new();
        private List<Personal> Administradores = new();
        private List<Bolsa>? Bolsas { get; set; }
        private KeyValuePair<string, string>[] Turnos;
        private string? BlobUrl { get; set; }
        private bool Render;
        private bool HideLoader = true;
        private bool HideError = true;

        public CajeroReceptorIndex(IReportesService reportesService, IJSInProcessRuntime js)
        {
            _reportesService = reportesService;
            _js = js;
        }

        protected override async Task OnInitializedAsync()
        {
            ReporteCajeroReceptorModel = new() { Fecha = DateTime.Now };
            EditContext = new EditContext(ReporteCajeroReceptorModel);

            UsuarioPlaza = await _reportesService.GetUsuarioPlazaAsync();
            ReporteCajeroReceptorModel.NumPlaza = UsuarioPlaza?.NumPlaza;
            ReporteCajeroReceptorModel.NumDelegacion = UsuarioPlaza?.NumDelegacion;

            var plazas = await _reportesService.GetPlazasAsync();
            Plaza = plazas.FirstOrDefault(x => x.NumPlaza == UsuarioPlaza.NumPlaza);

            var delegaciones = await _reportesService.GetDelegacionesAsync();
            Delegacion = delegaciones.FirstOrDefault(x => x.NumDelegacion == UsuarioPlaza.NumDelegacion);

            Administradores = await _reportesService.GetAdministradoresAsync();
            ReporteCajeroReceptorModel.NumGeaAdministrador = Administradores?.FirstOrDefault().NumGea;

            Turnos = await _reportesService.GetTurnosAsync();
            ReporteCajeroReceptorModel.IdTurno = Turnos?.FirstOrDefault().Key;

            Render = true;
        }

        private async Task ConsultarBolsas()
        {
            HideError = true;
            HideLoader = false;

            if (EditContext.Validate())
            {
                Bolsas = await _reportesService.CreateBolsasCajeroReceptorAsync(ReporteCajeroReceptorModel);
                if (Bolsas == null || Bolsas.Count <= 0)
                {
                    HideError = false;
                }
                if (!string.IsNullOrEmpty(BlobUrl))
                {
                    _js.Invoke<string>("RevokeObjectURL", BlobUrl);
                    BlobUrl = null;
                }
            }
            else
            {
                Console.WriteLine("Form is Invalid");
            }

            HideLoader = true;
        }
        private async Task GenerarReporte(int? id)
        {
            HideLoader = false;
            Bolsas = null;

            ReporteCajeroReceptorModel.IdBolsa = id;
            var content = await _reportesService.CreateReporteCajeroReceptorAsync(ReporteCajeroReceptorModel);
            BlobUrl = _js.Invoke<string>("CreateObjectURL", content, "ReporteCajeroReceptor.pdf", "application/pdf");

            HideLoader = true;
        }
    }
}
