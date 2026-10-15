using System.Reflection;
using System.Xml.Linq;

namespace Timeline.Domain.Tests;

/// <summary>
/// Confere a regra de dependência da seção 4 do arquitetura.md: o Domain não depende de
/// nenhuma outra camada nem de framework de infraestrutura.
/// </summary>
public class EstruturaDasCamadasTests
{
    private const string NomeDaSolucao = "TimelineEntregas.sln";

    private static readonly string[] PrefixosProibidos =
    [
        "Timeline.Application",
        "Timeline.Infrastructure",
        "Timeline.Api",
        "TimelineEntregas.Host",
        "Microsoft.EntityFrameworkCore",
        "Npgsql",
        "Microsoft.AspNetCore",
    ];

    /// <summary>
    /// Olha o que o projeto declara: sem referência a outros projetos, sem pacotes e sem
    /// framework adicional. Pega a dependência mesmo que ainda não seja usada no código.
    /// </summary>
    [Fact]
    public void Arq4_ProjetoDomain_NaoDeclaraReferenciaNemPacote()
    {
        var projeto = XDocument.Load(CaminhoDoProjetoDomain());

        var dependencias = projeto
            .Descendants()
            .Where(e => e.Name.LocalName is "ProjectReference" or "PackageReference" or "FrameworkReference")
            .Select(e => e.Attribute("Include")?.Value)
            .ToList();

        Assert.Empty(dependencias);
    }

    /// <summary>
    /// Olha o que o código compilado realmente usa: nenhuma outra camada do módulo e nenhum
    /// framework de infraestrutura (banco de dados ou web).
    /// </summary>
    [Fact]
    public void Arq4_AssemblyDomain_NaoUsaOutrasCamadasNemInfraestrutura()
    {
        var assemblyDomain = Assembly.Load("Timeline.Domain");

        var proibidas = assemblyDomain
            .GetReferencedAssemblies()
            .Select(a => a.Name ?? string.Empty)
            .Where(nome => PrefixosProibidos.Any(prefixo => nome.StartsWith(prefixo, StringComparison.Ordinal)))
            .ToList();

        Assert.Empty(proibidas);
    }

    private static string CaminhoDoProjetoDomain()
    {
        var pasta = new DirectoryInfo(AppContext.BaseDirectory);
        while (pasta is not null && !File.Exists(Path.Combine(pasta.FullName, NomeDaSolucao)))
        {
            pasta = pasta.Parent;
        }

        Assert.True(pasta is not null, $"Não encontrei o arquivo {NomeDaSolucao} acima de {AppContext.BaseDirectory}.");

        return Path.Combine(pasta!.FullName, "src", "Modules", "Timeline", "Timeline.Domain", "Timeline.Domain.csproj");
    }
}
