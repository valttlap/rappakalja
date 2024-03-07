using System.ComponentModel;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Sanasoppa.Core.DTOs;
using Sanasoppa.Core.Services;

namespace Sanasoppa.API.Controllers;

public class SettingsController : ApiBaseController
{
    private readonly SettingsService _settingsService;

    public SettingsController(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpGet]
    [SwaggerResponse(HttpStatusCode.OK, typeof(SettingsDto), Description = "Returns Auth0 Settings")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, null, Description = "Internal server error")]
    [OpenApiOperation("GetAuth0Settings")]
    [Description("Returns Auth0 Settings")]
    public ActionResult GetSettingsAsync()
    {
        return Ok(_settingsService.GetSettings());
    }
}
