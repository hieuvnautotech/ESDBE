using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Hubs;
using ESD.Middlewares;
using ESD.Services.Common;
using ESD.SubscribeTableDependencies;
using Swashbuckle.AspNetCore.Filters;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Project1.Services;

var builder = WebApplication.CreateBuilder(args);

////Add services to the container.
//builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
//builder.Services.AddScoped<IPersonService, PersonService>();
//builder.Services.AddTransient<IUserInfoService, UserInfoService>();

////Auto adding services to the container.
builder.Services.RegisterServices(builder.Configuration);
builder.Services.AddSingleton<IIotDBService, IotDbService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsApi",
       // builder => builder.WithOrigins("http://localhost:300", "http://hl.autonsi.com")
        builder => builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
})
//.AddFluentValidation(options =>
//            {
//                // Validate child properties and root collection elements
//                options.ImplicitlyValidateChildProperties = true;
//                options.ImplicitlyValidateRootCollectionElements = true;

//                // Automatic registration of validators in assembly
//                options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//            })
;

builder.Services.AddHttpClient("expoToken", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://fcm.googleapis.com");
    //httpClient.DefaultRequestHeaders.Accept.Clear();

    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("key={0}", "AAAALXNYFN4:APA91bEaP-ooWeEHt5XhJ3NdJusnK3f9t45Q9cTK-appcQzE-a_zEQBgJ1ek9tUMSnC9huJft-PoOfCDLGhMW7Mf0Ro5Da-2KgLY8YnA9BIy5MLHTolnO4vIWTUuz8yLOBvMyG9ni6pp"));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/// <summary>
/// Config Authorization for Swagger
/// </summary>
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT", Version = "v1" });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement {
        { securitySchema, new[] { "Bearer" } }
    };

    options.AddSecurityRequirement(securityRequirement);

    //options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    //{
    //    Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
    //    Name = "Authorization",
    //    In = ParameterLocation.Header,
    //    Type = SecuritySchemeType.Http,
    //    Scheme = "bearer",
    //    Reference = new OpenApiReference
    //    {
    //        Type = ReferenceType.SecurityScheme,
    //        Id = "Bearer"
    //    }
    //});

    //options.OperationFilter<SecurityRequirementsOperationFilter>();
});

/// <summary>
/// Add Authentication
/// </summary>
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
//{
//    //string key = builder.Configuration.GetSection("Jwt:Key").Value;
//    opt.TokenValidationParameters = new TokenValidationParameters
//    {
//        //ValidateLifetime = true,

//        ValidateIssuer = true,
//        ValidIssuer = ConnectionString.ISSUER,

//        ValidateAudience = true,
//        ValidAudience = ConnectionString.AUDIENCE,

//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConnectionString.SECRET)),
//        ClockSkew = TimeSpan.Zero
//    };
//});


builder.Services.AddMemoryCache();
builder.Services.AddHostedService<InitializeCacheService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<SignalRHub>();

builder.Services.AddSingleton<SubscribeUserTableDependency>();
builder.Services.AddSingleton<SubscribeAppTableDependency>();
builder.Services.AddSingleton<SubscribeWOTableDependency>();

builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.DisableDataAnnotationsValidation = true;
});

var app = builder.Build();


app.UseMiddleware<JwtMiddleware>();

////config CORS
app.UseCors("CorsApi");
//app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("access-token", "refresh-token", "content-type"));

app.UseStaticFiles();

app.UseRouting();

//var path = Path.Combine(app.Environment.ContentRootPath, "Upload");
//if (!Directory.Exists(path))
//{
//    Directory.CreateDirectory(path);
//}

//app.UseStaticFiles(new StaticFileOptions
//{
//    RequestPath = "/VersionApp",
//    FileProvider = new PhysicalFileProvider(path)
//});

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationHub>("/notification");
    endpoints.MapHub<SignalRHub>("/signalr");
});

// Configure the HTTP request pipeline.
app.UseSwagger();
if (app.Environment.IsProduction())
{
    app.UseSwaggerUI(c =>
    {
        c.DefaultModelsExpandDepth(-1); // Disable swagger schemas at bottom
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jwt v1");
        c.RoutePrefix = string.Empty;
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}
else
{
    app.UseSwaggerUI(c =>
    {
        c.DefaultModelsExpandDepth(-1); // Disable swagger schemas at bottom
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jwt v1");
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

//app.UseAuthentication();

//app.UseAuthorization();
app.UseSqlTableDependency<SubscribeWOTableDependency>(ConnectionString.CONNECTIONSTRING);
app.UseSqlTableDependency<SubscribeUserTableDependency>(ConnectionString.CONNECTIONSTRING);
app.UseSqlTableDependency<SubscribeAppTableDependency>(ConnectionString.CONNECTIONSTRING);

app.MapControllers();

app.Run();
