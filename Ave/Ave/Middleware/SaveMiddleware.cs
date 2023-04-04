using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ave.API.Attributes;
using Ave.BLL.Interface;
using Ave.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ave.API.Middleware
{
    /// <summary>
    /// If applied to the controller or controller action
    /// will automatically save all changes using the <see cref="IUnitOfWork"/>
    /// </summary>
    public class SaveMiddleware : IMiddleware
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SaveMiddleware> _logger;

        public SaveMiddleware(IServiceProvider serviceProvider, ILogger<SaveMiddleware> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);

            var endpointMetadata = context.GetEndpoint()?.Metadata;
            var performAutosave = endpointMetadata?.GetMetadata<AutosaveAttribute>()?.PerformAutosave ?? false;

            if (performAutosave)
            {
                var unitOfWorks = _serviceProvider.GetServices<IUnitOfWork>();

                try
                {

                    foreach (var unitOfWork in unitOfWorks)
                    {
                        await unitOfWork.SaveChangesAsync();
                    }

                    _logger.LogDebug($@"Changes of endpoint: ""{context.Request.Path}"" are saved");
                }
                catch (Exception)
                {
                    _logger.LogError($@"Error during saving changes of endpoint: ""{context.Request.Path}""");
                    throw;
                }
            }
        }
    }
}
