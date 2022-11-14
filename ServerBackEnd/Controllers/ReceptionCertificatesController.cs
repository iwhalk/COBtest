using ApiGateway.Interfaces;
using ApiGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz.Util;
using Shared.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReceptionCertificatesController : ControllerBase
    {
        private readonly IReceptionCertificateService _receptionCertificateService;
        public ReceptionCertificatesController(IReceptionCertificateService receptionCertificateService)
        {
            _receptionCertificateService = receptionCertificateService;
        }

        [HttpGet]
        public async Task<ActionResult> GetReceptionCertificates(string? day, string? week, string? month, string? propertyType, string? numberOfRooms, string? lessor, string? tenant, string? delegation, string? agent, string? currentPage, string? rowNumber)
        {
            day = GetNullableString(day);
            week = GetNullableString(week);
            month = GetNullableString(month);
            delegation = GetNullableString(delegation);
            agent = GetNullableString(agent);

            int? currentPageInt = null;
            int? rowNumberInt = null;

            if (!string.IsNullOrWhiteSpace(currentPage) && !string.IsNullOrWhiteSpace(rowNumber))
            {
                if (!int.TryParse(currentPage.Trim(), out int pa) || !int.TryParse(rowNumber.Trim(), out int ndf))
                {
                    return BadRequest($"La paginacion se encuentra en un formato incorrecto");
                }
                currentPageInt = pa;
                rowNumberInt = ndf;
            }

            int propertyTypeInt = 0;
            int numberOfRoomsInt = 0;
            int lessorInt = 0;
            int tenantInt = 0;

            if (!string.IsNullOrWhiteSpace(propertyType)) 
            {
                propertyTypeInt = Convert.ToInt16(propertyType); 
            }
            if (!string.IsNullOrWhiteSpace(numberOfRooms)) 
            {
                propertyTypeInt = Convert.ToInt16(numberOfRoomsInt); 
            }
            if (!string.IsNullOrWhiteSpace(lessor))
            {
                propertyTypeInt = Convert.ToInt16(lessorInt); 
            }
            if (!string.IsNullOrWhiteSpace(tenant))
            {
                propertyTypeInt = Convert.ToInt16(tenantInt); 
            }

            var result = await _receptionCertificateService.GetReceptionCertificateAsync(day, week, month, propertyTypeInt, numberOfRoomsInt, lessorInt, tenantInt, delegation, agent, currentPageInt, rowNumberInt);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostReceptionCertificates(ReceptionCertificate reception)
        {
            var result = await _receptionCertificateService.PostReceptionCertificateAsync(reception);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        static string? GetNullableString(string? value) => !string.IsNullOrWhiteSpace(value) && value.ToUpper().Contains("NULL") ? null : value;
    }
}
