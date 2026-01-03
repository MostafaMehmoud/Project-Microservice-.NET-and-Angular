

using Asp.Versioning;
using catalog.Application.Mappers;
using catalog.Application.Queries;
using catalog.Core.Repositories;
using catalog.Infrastructure.Data.Context;
using catalog.Infrastructure.Repositories;
using Common.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Logging.ConfiguraLogger);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.WebHost.UseUrls("http://0.0.0.0:8000");

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.Authority = "http://identityserver:9011"; // eShop.Identity server URL
//        options.RequireHttpsMetadata = false;
//        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            ValidateAudience = true,
//            ValidAudience = "catalogAPI",
//            ValidateIssuer = true,
//            ValidateLifetime = true,
//            ValidIssuer = "http://identityserver:9011",
//            ValidateIssuerSigningKey = true,
//            ClockSkew = TimeSpan.Zero
//        };

//        //add this to docker desktop https issue
//        options.BackchannelHttpHandler = new HttpClientHandler
//        {
//            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
//        };
//        options.Events=new JwtBearerEvents
//        {
//            OnAuthenticationFailed = context =>
//            {
//                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
//                logger.LogError("=============================================Authentication failed: {0}", context.Exception.Message);
//                return Task.CompletedTask;
//            },
//            OnTokenValidated = context =>
//            {
//                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
//                logger.LogInformation("==================================================Token validated for {0}", context.Principal.Identity.Name);
//                return Task.CompletedTask;
//            }
//        };
//    });
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("CanRead", policy => policy.RequireClaim("scope", "CalelogAPIScope.read"));
//});

    builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    Assembly.GetExecutingAssembly()
    ,Assembly.GetAssembly(typeof(GetProductByIdQuery))
    ));
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository,ProductRepositories>();
builder.Services.AddScoped<IBrandRepository, ProductRepositories>();
builder.Services.AddScoped<ITypeRepository, ProductRepositories>();
//var usePolicy=new AuthorizationPolicyBuilder()
//    .RequireAuthenticatedUser()
//    .Build();   
//builder.Services.AddControllers(options =>
//{
//    options.Filters.Add(new AuthorizeFilter(usePolicy));
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Catlog API",
        Description = "An ASP.NET Core Web API for managing Catlog items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Mostafa Mahmoud",
            Url = new Uri("https://example.com/contact")
        }


    });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapControllers(); // أو MapGet/MapPost إلخ


// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // BEFORE UseSwagger / routing
    app.Use((ctx, next) =>
    {
        if (ctx.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var p) && !string.IsNullOrEmpty(p))
            ctx.Request.PathBase = p.ToString();   // e.g., "/catalog"
        return next();
    });

    app.UseSwagger(c =>
    {
        // Make the OpenAPI "servers" base path match the prefix so Try it out uses /catalog/...
        c.PreSerializeFilters.Add((doc, req) =>
        {
            var prefix = req.Headers["X-Forwarded-Prefix"].FirstOrDefault();
            if (!string.IsNullOrEmpty(prefix))
                doc.Servers = new List<Microsoft.OpenApi.Models.OpenApiServer>
            { new() { Url = prefix } };
        });
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Catalog.API v1"); // relative path (no leading '/')
        c.RoutePrefix = "swagger";
    });

    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
