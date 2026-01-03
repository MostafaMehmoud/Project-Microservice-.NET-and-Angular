using Asp.Versioning;
using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Mappers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Logging.ConfiguraLogger);   
// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.Authority = "http://identityserver:9011";
//        options.RequireHttpsMetadata = false;

//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidIssuer = "http://identityserver:9011", // خليه كما هو لأن التوكن يستخدم هذا الـ issuer

//            ValidateAudience = true,
//            ValidAudiences = new[] { "Basket", "EShoppingGateway" },

//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ClockSkew = TimeSpan.Zero
//        };
//        //Add this to docker to host communtication
//        options.BackchannelHttpHandler = new HttpClientHandler
//        {
//            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
//        };

//        options.Events = new JwtBearerEvents
//        {
//            OnAuthenticationFailed = context =>
//            {
//                Console.WriteLine($"======= AUTHENTICTION FAILED");
//                Console.WriteLine($"Exception :{context.Exception.Message}");
//                Console.WriteLine($"Authority:{options.Authority}");
//                return Task.CompletedTask;
//            }
//        };

//    });
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddAutoMapper(typeof(BasketMappingProfile).Assembly);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    Assembly.GetExecutingAssembly()
    , Assembly.GetAssembly(typeof(CreateShoppingCartCommand))
    ));

builder.Services.AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Basket API",
        Description = "An ASP.NET Core Web API for managing Basket items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Mostafa Mahmoud",
            Url = new Uri("https://example.com/contact")
        }


    });
    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v2",
        Title = "Basket API",
        Description = "An ASP.NET Core Web API for managing Basket items foe version 2",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Mostafa Mahmoud",
            Url = new Uri("https://example.com/contact")
        }


    });

    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (!apiDesc.TryGetMethodInfo(out var methodInfo))
            return false;

        var versions = methodInfo.DeclaringType!
            .GetCustomAttributes(true)
            .OfType<ApiVersionAttribute>()
            .SelectMany(v => v.Versions);

        // ?????? ??? MajorVersion ???????
        return versions.Any(v => $"v{v.ToString()}" == docName);
    });


});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

//var userPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser().Build();

//builder.Services.AddControllers(config =>
//{
//    config.Filters.Add(new AuthorizeFilter(userPolicy));
//});
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    grp=>grp.Address=new Uri(builder.Configuration["GrpcSettings:DiscountUrl"])
    );
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });

});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMassTransitHostedService();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var nginxPath="/basket";
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.Use((ctx, next) =>
    {
        if (ctx.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefix) &&
            !string.IsNullOrEmpty(prefix))
        {
            ctx.Request.PathBase = prefix.ToString(); // e.g., "/basket"
        }
        return next();
    });
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Use *relative* URLs so the /basket prefix is preserved by the browser
        c.SwaggerEndpoint("v1/swagger.json", "Basket.API v1");   // no leading '/'
        c.SwaggerEndpoint("v2/swagger.json", "Basket.API v2");   // no leading '/'
        c.RoutePrefix = "swagger";

    });
}
app.UseCors("CorsPolicy");
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
