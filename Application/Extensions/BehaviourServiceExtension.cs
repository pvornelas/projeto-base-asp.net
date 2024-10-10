using Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class BehaviourServiceExtension
    {
        public static IServiceCollection AddBehaviourConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(static opt =>
            {
                opt.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new
                    {
                        campo = e.Key,
                        erros = string.Join(",", e.Value.Errors.Select(er => er.ErrorMessage))
                    });

                    var response = RequestResult<Exception>.FailureResult("Erros de validação", string.Join(",", errors));
                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
    }
}
