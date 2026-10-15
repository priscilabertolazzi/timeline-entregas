using Timeline.Application.Aplicacoes;
using Timeline.Application.Erros;

namespace Timeline.Application.Tests;

/// <summary>Casos de uso de aplicações (RF-01, RN-01, RN-04), com os textos exatos do catálogo.</summary>
public sealed class AplicacaoCasosDeUsoTests
{
    private readonly AplicacaoRepositoryEmMemoria _repositorio = new();

    private Task<AplicacaoResposta> Cadastrar(string? nome) =>
        new CadastrarAplicacao(_repositorio).ExecutarAsync(new NomeAplicacaoEntrada(nome), CancellationToken.None);

    private Task<AplicacaoResposta> Renomear(long id, string? nome) =>
        new RenomearAplicacao(_repositorio).ExecutarAsync(id, new NomeAplicacaoEntrada(nome), CancellationToken.None);

    private Task Excluir(long id) =>
        new ExcluirAplicacao(_repositorio).ExecutarAsync(id, CancellationToken.None);

    private async Task<IReadOnlyList<string>> NomesListados() =>
        (await new ListarAplicacoes(_repositorio, new VersaoRepositoryEmMemoria()).ExecutarAsync(CancellationToken.None)).Select(a => a.Nome).ToList();

    // ---------- Cadastrar ----------

    [Fact]
    public async Task RF01_CadastrarAlfa_DevolveAplicacaoComIdENome()
    {
        var resposta = await Cadastrar("Alfa");

        Assert.Equal("Alfa", resposta.Nome);
        Assert.True(resposta.Id > 0);
        Assert.Equal(["Alfa"], await NomesListados());
    }

    [Fact]
    public async Task RN01_CadastrarComEspacosNasPontas_GuardaSemOsEspacos()
    {
        var resposta = await Cadastrar("  Beta  ");

        Assert.Equal("Beta", resposta.Nome);
    }

    [Theory]
    [InlineData("alfa")]
    [InlineData("ALFA")]
    [InlineData("  Alfa  ")]
    [InlineData("Alfa")]
    public async Task CA33_CadastrarNomeIgualAoDeAlfa_RecusaComMsg03(string nomeDuplicado)
    {
        await Cadastrar("Alfa");

        var erro = await Assert.ThrowsAsync<ConflitoException>(() => Cadastrar(nomeDuplicado));

        Assert.Equal(["Já existe uma aplicação com esse nome."], erro.Mensagens);
        Assert.Equal(["Alfa"], await NomesListados());
    }

    [Fact]
    public async Task CA33_DoisCadastrosSimultaneosComOMesmoNome_BancoRecusaEResultaEmMsg03()
    {
        await Cadastrar("Alfa");
        _repositorio.BancoRecusaDuplicidadeNaGravacao = true;

        var erro = await Assert.ThrowsAsync<ConflitoException>(() => Cadastrar("alfa"));

        Assert.Equal(["Já existe uma aplicação com esse nome."], erro.Mensagens);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RN01_CadastrarNomeVazioOuSoEspacos_RecusaComMsg01(string? nome)
    {
        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => Cadastrar(nome));

        Assert.Equal(["Informe o nome da aplicação."], erro.Mensagens);
        Assert.Empty(_repositorio.Gravadas);
    }

    [Fact]
    public async Task RN01_CadastrarSemCorpoNaRequisicao_RecusaComMsg01()
    {
        var caso = new CadastrarAplicacao(_repositorio);

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => caso.ExecutarAsync(null, CancellationToken.None));

        Assert.Equal(["Informe o nome da aplicação."], erro.Mensagens);
    }

    [Fact]
    public async Task Msg02_CadastrarNomeCom101Caracteres_RecusaComMsg02()
    {
        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => Cadastrar(new string('A', 101)));

        Assert.Equal(["O nome da aplicação deve ter no máximo 100 caracteres."], erro.Mensagens);
        Assert.Empty(_repositorio.Gravadas);
    }

    [Fact]
    public async Task Msg02_CadastrarNomeCom100Caracteres_Aceita()
    {
        var resposta = await Cadastrar(new string('A', 100));

        Assert.Equal(100, resposta.Nome.Length);
    }

    [Fact]
    public async Task Msg02_CadastrarNomeCom100CaracteresMaisEspacosNasPontas_Aceita()
    {
        var resposta = await Cadastrar("  " + new string('A', 100) + "  ");

        Assert.Equal(100, resposta.Nome.Length);
    }

    // ---------- Listar ----------

    [Fact]
    public async Task RF01_ListarSemAplicacoes_DevolveListaVazia()
    {
        Assert.Empty(await NomesListados());
    }

    [Fact]
    public async Task RF01_ListarAplicacoes_DevolveEmOrdemAlfabeticaSemDiferenciarMaiusculas()
    {
        await Cadastrar("gama");
        await Cadastrar("Delta");
        await Cadastrar("Beta");
        await Cadastrar("Alfa");

        Assert.Equal(["Alfa", "Beta", "Delta", "gama"], await NomesListados());
    }

    // ---------- Renomear ----------

    [Fact]
    public async Task RF01_RenomearAlfaParaGama_AlteraONome()
    {
        var alfa = await Cadastrar("Alfa");

        var resposta = await Renomear(alfa.Id, "  Gama ");

        Assert.Equal(new AplicacaoResposta(alfa.Id, "Gama"), resposta);
        Assert.Equal(["Gama"], await NomesListados());
    }

    [Fact]
    public async Task CA33_RenomearAlfaParaAlfaMinusculo_AceitaPoisEAMesmaAplicacao()
    {
        var alfa = await Cadastrar("Alfa");

        var resposta = await Renomear(alfa.Id, "alfa");

        Assert.Equal("alfa", resposta.Nome);
    }

    [Fact]
    public async Task CA33_RenomearAlfaParaONomeDeBetaEmOutraCaixa_RecusaComMsg03()
    {
        var alfa = await Cadastrar("Alfa");
        await Cadastrar("Beta");

        var erro = await Assert.ThrowsAsync<ConflitoException>(() => Renomear(alfa.Id, "BETA"));

        Assert.Equal(["Já existe uma aplicação com esse nome."], erro.Mensagens);
        Assert.Equal(["Alfa", "Beta"], await NomesListados());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RN01_RenomearParaNomeVazio_RecusaComMsg01(string nome)
    {
        var alfa = await Cadastrar("Alfa");

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => Renomear(alfa.Id, nome));

        Assert.Equal(["Informe o nome da aplicação."], erro.Mensagens);
        Assert.Equal(["Alfa"], await NomesListados());
    }

    [Fact]
    public async Task Msg02_RenomearParaNomeCom101Caracteres_RecusaComMsg02()
    {
        var alfa = await Cadastrar("Alfa");

        var erro = await Assert.ThrowsAsync<EntradaInvalidaException>(() => Renomear(alfa.Id, new string('B', 101)));

        Assert.Equal(["O nome da aplicação deve ter no máximo 100 caracteres."], erro.Mensagens);
    }

    [Fact]
    public async Task Msg04_RenomearAplicacaoInexistente_RecusaComMsg04()
    {
        var erro = await Assert.ThrowsAsync<NaoEncontradoException>(() => Renomear(999, "Alfa"));

        Assert.Equal(["Aplicação não encontrada."], erro.Mensagens);
    }

    // ---------- Excluir ----------

    [Fact]
    public async Task CA38_ExcluirDelta_ElaDeixaDeAparecerNaLista()
    {
        await Cadastrar("Alfa");
        var delta = await Cadastrar("Delta");

        await Excluir(delta.Id);

        Assert.Equal(["Alfa"], await NomesListados());
    }

    [Fact]
    public async Task RN04_ExcluirEDepoisCadastrarOMesmoNome_Aceita()
    {
        var delta = await Cadastrar("Delta");
        await Excluir(delta.Id);

        var novaDelta = await Cadastrar("Delta");

        Assert.Equal("Delta", novaDelta.Nome);
    }

    [Fact]
    public async Task Msg04_ExcluirAplicacaoInexistente_RecusaComMsg04()
    {
        var erro = await Assert.ThrowsAsync<NaoEncontradoException>(() => Excluir(999));

        Assert.Equal(["Aplicação não encontrada."], erro.Mensagens);
    }

    [Fact]
    public async Task Msg04_ExcluirDuasVezesAMesmaAplicacao_SegundaVezRecusaComMsg04()
    {
        var delta = await Cadastrar("Delta");
        await Excluir(delta.Id);

        await Assert.ThrowsAsync<NaoEncontradoException>(() => Excluir(delta.Id));
    }
}
