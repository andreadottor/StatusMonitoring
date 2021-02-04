namespace Microsoft.Extensions.DependencyInjection
{
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static  class HealthChecksConfiguration
    {
        public static void AddConfiguredHealthChecks(this IServiceCollection services, string connectionString)
        {
            services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: connectionString,
                    healthQuery: "SELECT 1;",
                    name: "Azure SQL Server",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new string[] { "db", "sql", "sqlserver" });


            services.AddHealthChecksUI(setup =>
            {
                setup.AddHealthCheckEndpoint("Local Services", "/healthz");

                setup.SetEvaluationTimeInSeconds(30);
                setup.SetMinimumSecondsBetweenFailureNotifications(60);
            }).AddSqlServerStorage(connectionString);
        }

        public static IEndpointConventionBuilder MapConfiguredHealthChecksUI(this IEndpointRouteBuilder builder)
        {
            builder.MapHealthChecks("healthz", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return builder.MapHealthChecksUI(setup =>
            {
                // https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/doc/styles-branding.md
                //
                setup.AddCustomStylesheet("HealthUI.css");
                setup.AsideMenuOpened = true;
            }).RequireAuthorization();
        }
    }
}
