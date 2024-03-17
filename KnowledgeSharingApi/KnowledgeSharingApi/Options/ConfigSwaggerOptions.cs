using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KnowledgeSharingApi.Options
{
    public class ConfigSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            var schema = GetJwtSecuritySchema();
            options.AddSecurityDefinition(schema.Reference.Id, schema);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { schema, Array.Empty<string>() },
            });
        }

        private static OpenApiSecurityScheme GetJwtSecuritySchema()
        {
            return new OpenApiSecurityScheme
            {
                Name = "Jwt Authentication",
                Description = "Provide a JWT Bearer",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                },
            };
        }

    }
}