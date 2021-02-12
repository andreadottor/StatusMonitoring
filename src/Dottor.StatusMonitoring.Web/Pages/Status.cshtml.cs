using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Dottor.StatusMonitoring.Web.Pages
{
    public class StatusModel : PageModel
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthReport Report { get; private set; }

        public StatusModel(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        public async Task OnGet()
        {
            Report = await _healthCheckService.CheckHealthAsync();
        }

    }
}