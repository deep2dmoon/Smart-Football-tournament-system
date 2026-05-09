using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using smarttournamentengine.STE.Application;
using smarttournamentengine.STE.Application.Engine.Components;
using smarttournamentengine.STE.Controller.Endpoints;
using smarttournamentengine.STE.Controller.Endpoints.Auth;
using smarttournamentengine.STE.infrastructure;
using smarttournamentengine.STE.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<TournamentEngine>();
builder.Services.AddScoped<FixtureEngine>();
builder.Services.AddScoped<GroupEngine>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ParticipantOnboardEngine>();
builder.Services.AddScoped<StandingUpdateComponent>();

builder.Host.UseSerilog((context, services, config) =>
{
    config.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);
});

builder.Services.AddDbContext<DatabaseContext>((option) => option.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnectionString")));
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", (options) =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Just a random strings put togther to get asymmetrical key")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});
builder.Services.AddAuthorization();


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapTournamentEndpoints();
app.MapAuthEndpoints();

app.Run();
