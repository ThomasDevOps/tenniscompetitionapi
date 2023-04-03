using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "TennisCompetitionApi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference
                {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
            }
    });
});
builder.Services.AddDbContext<TennisDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TennisDb"));
});
builder.Services.AddAuthentication(sharedoptions => sharedoptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = string.Format("{0}{1}", builder.Configuration["AzureAd:Instance"], builder.Configuration["AzureAd:TenantId"]);
    options.Audience = builder.Configuration["AzureAd:ClientId"];
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters { ValidIssuers = new List<string> { "https://login.microsoftonline.com/e612b9ec-3885-4cc0-a3fa-45981fc8be64/v2.0" }};
});
builder.Services.AddAuthorization();
builder.Services.AddCors();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<TennisDbContext>();
dbContext.Database.Migrate();

app.MapAllEndpoints(dbContext);
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TennisCompetitionApi v1"));
if (app.Environment.IsDevelopment())
{
    // Set CORS for Localhost
    app.UseCors(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
        policy.SetIsOriginAllowed(isOriginAllowed: _ => true);
    });
}
app.UseAuthentication();
app.UseAuthorization();
app.Run();
