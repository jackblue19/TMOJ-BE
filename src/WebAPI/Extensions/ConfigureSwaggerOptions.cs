using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI.Extensions;

sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach ( var desc in _provider.ApiVersionDescriptions )
        {
            options.SwaggerDoc(desc.GroupName , new OpenApiInfo
            {
                Title = "Themis Online Judge API" ,
                Version = desc.GroupName ,
                Description = desc.IsDeprecated ? "This TMOJ API version has been deprecated." : null
            });
        }
    }
}


