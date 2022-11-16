using ApiGateway.Interfaces;
using ApiGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz.Util;
using SharedLibrary.Models;

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
        public async Task<ActionResult> GetReceptionCertificates(string? startDay, string? endDay, string? certificateType, string? propertyType, string? numberOfRooms, string? lessor, string? tenant, string? delegation, string? agent, string? currentPage, string? rowNumber)
        {
            startDay = GetNullableString(startDay);
            endDay = GetNullableString(endDay);
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

            int certificateTypeInt = 0;
            int propertyTypeInt = 0;
            int numberOfRoomsInt = 0;
            int lessorInt = 0;
            int tenantInt = 0;

            if (!string.IsNullOrWhiteSpace(certificateType))
            {
                certificateTypeInt = Convert.ToInt16(certificateType);
            }
            if (!string.IsNullOrWhiteSpace(propertyType)) 
            {
                propertyTypeInt = Convert.ToInt16(propertyType); 
            }
            if (!string.IsNullOrWhiteSpace(numberOfRooms)) 
            {
                numberOfRoomsInt = Convert.ToInt16(numberOfRooms); 
            }
            if (!string.IsNullOrWhiteSpace(lessor))
            {
                lessorInt = Convert.ToInt16(lessor); 
            }
            if (!string.IsNullOrWhiteSpace(tenant))
            {
                tenantInt = Convert.ToInt16(tenant); 
            }

            var result = await _receptionCertificateService.GetReceptionCertificateAsync(startDay, endDay, certificateTypeInt, propertyTypeInt, numberOfRoomsInt, lessorInt, tenantInt, delegation, agent, currentPageInt, rowNumberInt);

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

        [HttpPut]
        public async Task<ActionResult> PutReceptionCertificates(ReceptionCertificate reception)
        {
            var result = await _receptionCertificateService.PutReceptionCertificateAsync(reception);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        static string? GetNullableString(string? value) => !string.IsNullOrWhiteSpace(value) && value.ToUpper().Contains("NULL") ? null : value;
    }
}
