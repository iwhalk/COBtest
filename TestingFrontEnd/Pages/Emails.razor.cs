﻿using FrontEnd.Components;
using FrontEnd.Interfaces;
using FrontEnd.Services;
using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLibrary.Models;
using System.Reflection.Metadata.Ecma335;

namespace FrontEnd.Pages
{
    public partial class Emails : ComponentBase
    {
        private readonly IMailObraService _mailAriService;
        private readonly ApplicationContext _context;
        private readonly ILessorService _lessorService;
        private readonly ITenantService _tenantService;
        private readonly IPropertyService _propertyService;
        private readonly IReportsService _reportService;
        public Emails(IMailObraService mailAriService, ApplicationContext context, ILessorService lessorService, ITenantService tenantService, IPropertyService propertyService, IReportsService reportService)
        {
            _mailAriService = mailAriService;
            _context = context;
            _lessorService = lessorService;
            _tenantService = tenantService;
            _propertyService = propertyService;
            _reportService = reportService;
        }

        public bool ShowModalSend { get; set; }
        public void ChangeOpenModalSend() => ShowModalSend = ShowModalSend ? false : true;

        private List<Lessor> lessors { get; set; }
        private List<Tenant> tenants { get; set; }

        private List<Property> properties;
        private Property property;

        private string? tenant { get; set; } = null;
        private string? lessor { get; set; } = null;
        private string? agencia { get; set; } = null;
        private string? agente { get; set; } = null;
        private string? otro { get; set; } = null;

        private bool? tenantCheck { get; set; } = null;
        private bool? lessorCheck { get; set; } = null;
        private bool? agenciaCheck { get; set; } = null;
        private bool? agenteCheck { get; set; } = null;
        private bool? otroCheck { get; set; } = null;
        public bool ShowModalPreview { get; set; }
        public bool DisablePreView { get; set; } = false;
        public string PdfName { get; set; } = null;
        public byte[] BlobPDFPreview { get; set; }
        public ReceptionCertificate CurrentReceptionCertificate;

        public void ChangeOpenModalPreview() => ShowModalPreview = ShowModalPreview ? false : true;

        protected override async Task OnInitializedAsync()
        {
            await GetLessorTenantAddress();
        }
        private async Task GetLessorTenantAddress()
        {
            CurrentReceptionCertificate = _context.CurrentReceptionCertificate ?? _context.ReceptionCertificateExist;

            lessors = await _lessorService.GetLessorAsync();
            tenants = await _tenantService.GetTenantAsync();
            properties = await _propertyService.GetPropertyAsync();
            tenant = tenants.FirstOrDefault(x=>x.IdTenant == CurrentReceptionCertificate.IdTenant).EmailAddress;
            property = properties.FirstOrDefault(x=>x.IdProperty == CurrentReceptionCertificate.IdProperty);
            lessor = lessors.FirstOrDefault(x=>x.IdLessor == property.IdLessor).EmailAddress;
            agente = _context.CurrentUser.Email;
            //tenant = tenants.FirstOrDefault(x => x.IdTenant == _context.CurrentReceptionCertificate.IdTenant).EmailAddress;
            //property = properties.FirstOrDefault(x => x.IdLessor == _context.CurrentReceptionCertificate.IdProperty);
            //lessor = lessors.FirstOrDefault(x => x.IdLessor == property.IdLessor).EmailAddress;
        }
        public async void HandleSaveReceptionCertificate()
        {
            SendMenssage();
        }
        public async void HandlePreviewPdf()
        {
            DisablePreView = true;
            if (CurrentReceptionCertificate != null)
            {
                var IdReceptionCertificate = CurrentReceptionCertificate.IdReceptionCertificate;
                BlobPDFPreview = await _reportService.GetReporteReceptionCertificate(IdReceptionCertificate);
                if (BlobPDFPreview != null)
                {                   
                    PdfName = "PDFPreview.pdf";
                    Thread.Sleep(5000);
                }
                StateHasChanged();
            }
            DisablePreView = false;
            ShowModalPreview = true;
            StateHasChanged();
        }
        private async Task SendMenssage()
        {
            int idReceptionCertificate = CurrentReceptionCertificate.IdReceptionCertificate;
            if (idReceptionCertificate > 0)
            {
                if (tenantCheck == true)
                {
                    _mailAriService.GetMailAsync(idReceptionCertificate, tenant);
                }
                if (lessorCheck == true)
                {                    
                    _mailAriService.GetMailAsync(idReceptionCertificate, lessor);
                }
                if (agenciaCheck == true)
                {
                    _mailAriService.GetMailAsync(idReceptionCertificate, agencia);
                }
                if (agenteCheck == true)
                {
                    _mailAriService.GetMailAsync(idReceptionCertificate, agente);
                }
                if (otroCheck == true)
                {
                    _mailAriService.GetMailAsync(idReceptionCertificate, otro);
                }
                ChangeOpenModalSend();
            }

            return;            
        }
    }
}
