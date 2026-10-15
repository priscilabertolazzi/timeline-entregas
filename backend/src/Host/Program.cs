using Timeline.Infrastructure;
using TimelineEntregas.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTimelineInfrastructure(ConexaoBanco.Montar(builder.Configuration));
builder.Services.AddHostedService<MigracoesNaInicializacao>();

var app = builder.Build();

app.Run();
