# Imagem da API (arquitetura.md, seção 12.1). Contexto de construção: a raiz do projeto.
# Etapa 1: compila com o SDK do .NET. Etapa 2: imagem leve, só para executar.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS compilacao
WORKDIR /origem
COPY global.json ./
COPY .editorconfig ./
COPY backend/ ./backend/
RUN dotnet publish backend/src/Host/TimelineEntregas.Host.csproj -c Release -o /publicado

FROM mcr.microsoft.com/dotnet/aspnet:10.0
# Os dados de fuso horário precisam existir na imagem para a variável TZ funcionar (seção 5.6).
RUN apt-get update \
    && apt-get install -y --no-install-recommends tzdata \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=compilacao /publicado ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "TimelineEntregas.Host.dll"]
