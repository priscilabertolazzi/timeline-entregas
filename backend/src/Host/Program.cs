using Timeline.Api;
using Timeline.Infrastructure;
using TimelineEntregas.Host;

var builder = WebApplication.CreateBuilder(args);

// A API só aceita chamadas do próprio computador (arquitetura.md, seção 10), na porta API_PORT do .env.
// No Docker, o endereço vem de ASPNETCORE_URLS e esta linha não age.
if (builder.Configuration["ASPNETCORE_URLS"] is null && builder.Configuration["URLS"] is null)
{
    builder.WebHost.UseUrls($"http://localhost:{builder.Configuration["API_PORT"] ?? "8080"}");
}

builder.Services.AddTimelineInfrastructure(ConexaoBanco.Montar(builder.Configuration));
builder.Services.AddHostedService<MigracoesNaInicializacao>();
builder.Services.AddTimelineApi();

var app = builder.Build();

app.UseTimelineApi();

app.Run();
