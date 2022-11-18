﻿using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLibrary.Models;
using System.Reflection.Metadata.Ecma335;

namespace FrontEnd.Pages
{
    public partial class Emails : ComponentBase
    {
        private readonly IMailAriService _mailAriService;
        private readonly ApplicationContext _context;
        private readonly ILessorService _lessorService;
        private readonly ITenantService _tenantService;
        private readonly IPropertyService _propertyService;
        public Emails(IMailAriService mailAriService, ApplicationContext context, ILessorService lessorService, ITenantService tenantService, IPropertyService propertyService)
        {
            _mailAriService = mailAriService;
            _context = context;
            _lessorService = lessorService;
            _tenantService = tenantService;
            _propertyService = propertyService;
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

        protected override async Task OnInitializedAsync()
        {
            await GetLessorTenantAddress();                                  
        }
        private async Task GetLessorTenantAddress()
        {
            lessors = await _lessorService.GetLessorAsync();
            tenants = await _tenantService.GetTenantAsync();
            properties = await _propertyService.GetPropertyAsync();
            tenant = _context.CurrentTenant.EmailAddress;
            property = _context.CurrentPropertys;
            lessor = _context.CurrentLessor.EmailAddress;
            agente = _context.CurrentUser.Email;
            //tenant = tenants.FirstOrDefault(x => x.IdTenant == _context.CurrentReceptionCertificate.IdTenant).EmailAddress;
            //property = properties.FirstOrDefault(x => x.IdLessor == _context.CurrentReceptionCertificate.IdProperty);
            //lessor = lessors.FirstOrDefault(x => x.IdLessor == property.IdLessor).EmailAddress;
        }

        private async Task SendMenssage()
        {
            int idProperty = _context.CurrentReceptionCertificate.IdProperty;
            if (idProperty > 0)
            {
                if (tenantCheck == true)
                {
                    _mailAriService.GetMailAsync(idProperty, tenant);
                }
                if (lessorCheck == true)
                {                    
                    _mailAriService.GetMailAsync(idProperty, lessor);
                }
                if (agenciaCheck == true)
                {
                    _mailAriService.GetMailAsync(idProperty, agencia);
                }
                if (agenteCheck == true)
                {
                    _mailAriService.GetMailAsync(idProperty, agente);
                }
                if (otroCheck == true)
                {
                    _mailAriService.GetMailAsync(idProperty, otro);
                }
                ChangeOpenModalSend();
            }
            return;            
        }
    }
}
