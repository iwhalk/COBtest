﻿using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class MailAriController : ControllerBase
    {
        private readonly IMailAriService _service;

        public MailAriController(IMailAriService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetMail(string idProperty, string email)
        {
            idProperty = GetNullableString(idProperty);
            email = GetNullableString(email);

            int idPropertyInt = 0;

            if (!string.IsNullOrWhiteSpace(idProperty))
            {
                idPropertyInt = Convert.ToInt16(idProperty);
            }
         
            var result = await _service.GetMailAsync(idPropertyInt, email);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        static string? GetNullableString(string? value) => !string.IsNullOrWhiteSpace(value) && value.ToUpper().Contains("NULL") ? null : value;
    }
}