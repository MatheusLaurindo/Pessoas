using Microsoft.EntityFrameworkCore;
using Pessoas.Server.DTOs.Request;
using Pessoas.Server.Enuns;
using Pessoas.Server.Infra;
using Pessoas.Server.Model;
using Pessoas.Server.Repositories;
using Pessoas.Server.Services;
using System.Globalization;

namespace Tests.ServiceTests.Pessoa;

public class PessoaServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }

    [Fact(DisplayName = "GetAllAsync - Deve retornar todas as pessoas")]
    public async Task Cenario_1()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        Seeding.SeedPessoas(contexto);

        // Act
        var resultado = await service.GetAllAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(15, resultado.Count());
    }

    [Fact(DisplayName = "GetByIdAsync - Deve retornar a pessoa com id válido")]
    public async Task Cenario_2()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.Server.Model.Pessoa.Create(
            "João",
            "joao@email.com",
            DateTime.UtcNow.AddYears(-25),
            "123.456.789-00",
            Sexo.Masculino,
            Nacionalidade.Brasileira,
            "SP",
            "Rua A").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.GetByIdAsync(pessoa.Id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(pessoa.Nome, resultado.Nome);
        Assert.Equal(pessoa.Email, resultado.Email);
    }

    [Fact(DisplayName = "GetByIdAsync - Deve retornar nulo se id inválido")]
    public async Task Cenario_3()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        // Act
        var resultado = await service.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(resultado);
    }

    [Fact(DisplayName = "GetAllPaginatedAsync - Deve retornar pessoas de forma paginada")]
    public async Task Cenario_4()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        Seeding.SeedPessoas(contexto);

        // Act
        var resultado = await service.GetAllPaginatedAsync(1, 5);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(5, resultado.Data.Count());
    }

    [Fact(DisplayName = "AddAsync - Deve adicionar pessoa com sucesso")]
    public async Task Cenario_5()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var request = new AdicionarPessoaRequest
        {
            Nome = "João",
            Email = "joao@email.com",
            DataNascimento = "20/09/1998",
            Cpf = "111.111.111-11",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.True(resultado.FoiSucesso);
        Assert.NotEqual(Guid.Empty, resultado.Valor.Id);
    }

    [Fact(DisplayName = "AddAsync - Deve falhar se cpf ja existir")]
    public async Task Cenario_6()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        Seeding.SeedPessoas(contexto);

        var request = new AdicionarPessoaRequest
        {
            Nome = "João",
            Email = "joao@email.com",
            DataNascimento = "20/09/1998",
            Cpf = "123.456.789-01",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
    }

    [Fact(DisplayName = "AddAsync - Deve falhar se cpf for inválido")]
    public async Task Cenario_7()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var request = new AdicionarPessoaRequest
        {
            Nome = "João",
            Email = "joao@email.com",
            DataNascimento = "20/09/1998",
            Cpf = "123.123.123.222.1",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact(DisplayName = "AddAsync - Deve falhar se email inválido")]
    public async Task Cenario_8()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var request = new AdicionarPessoaRequest
        {
            Nome = "João",
            Email = "emailinvalidoteste",
            DataNascimento = "20/09/1998",
            Cpf = "123.123.123-12",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact(DisplayName = "AddAsync - Deve falhar se nome inválido")]
    public async Task Cenario_9()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var request = new AdicionarPessoaRequest
        {
            Nome = "",
            Email = "email@gmail.com",
            DataNascimento = "20/09/1998",
            Cpf = "123.123.123-12",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact(DisplayName = "AddAsync - Deve falhar se data de nascimento inválida")]
    public async Task Cenario_10()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var request = new AdicionarPessoaRequest
        {
            Nome = "joao da silva",
            Email = "email@gmail.com",
            DataNascimento = DateTime.UtcNow.AddYears(1).ToShortDateString(),
            Cpf = "123.123.123-12",
            Sexo = Sexo.Masculino,
            Nacionalidade = Nacionalidade.Brasileira,
            Naturalidade = "RJ"
        };

        // Act
        var resultado = await service.AddAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact(DisplayName = "UpdateAsync - Deve atualizar dados da pessoa com sucesso")]
    public async Task Cenario_11()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.Server.Model.Pessoa.Create(
            "Ana", 
            "ana@email.com", 
            DateTime.Today.AddYears(-28), 
            "111.333.555-66", 
            Sexo.Feminino, 
            Nacionalidade.Brasileira, 
            "BA", 
            "Rua D").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        var request = new EditarPessoaRequest
        {
            Id = pessoa.Id,
            Nome = "Ana Atualizada",
            Email = "ana@nova.com",
            DataNascimento = Convert.ToDateTime(pessoa.DataNascimento).ToShortDateString(),
            Cpf = pessoa.Cpf,
            Sexo = pessoa.Sexo,
            Nacionalidade = pessoa.Nacionalidade,
            Naturalidade = pessoa.Naturalidade,
            Endereco = "Rua Nova"
        };

        // Act
        var resultado = await service.UpdateAsync(request);

        // Assert
        Assert.True(resultado.FoiSucesso);
        Assert.Equal(request.Nome, resultado.Valor.Nome);
        Assert.Equal(request.Email, resultado.Valor.Email);
        Assert.Equal(request.Endereco, resultado.Valor.Endereco);
    }

    [Fact(DisplayName = "UpdateAsync - Deve falhar ao atualizar dados da pessoa com email invalido")]
    public async Task Cenario_12()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.Server.Model.Pessoa.Create(
            "Ana",
            "ana@email.com",
            DateTime.Today.AddYears(-28),
            "111.333.555-66",
            Sexo.Feminino,
            Nacionalidade.Brasileira,
            "BA",
            "Rua D").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        var request = new EditarPessoaRequest
        {
            Id = pessoa.Id,
            Nome = "Ana Atualizada",
            Email = "ananova.com",
            DataNascimento = "11/11/2002",
            Cpf = pessoa.Cpf,
            Sexo = pessoa.Sexo,
            Nacionalidade = pessoa.Nacionalidade,
            Naturalidade = pessoa.Naturalidade,
            Endereco = "Rua Nova"
        };

        // Act
        var resultado = await service.UpdateAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact(DisplayName = "UpdateAsync - Deve falhar ao atualizar dados da pessoa com nome invalido")]
    public async Task Cenario_13()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.Server.Model.Pessoa.Create(
            "Ana",
            "ana@email.com",
            DateTime.Today.AddYears(-28),
            "111.333.555-66",
            Sexo.Feminino,
            Nacionalidade.Brasileira,
            "BA",
            "Rua D").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        var request = new EditarPessoaRequest
        {
            Id = pessoa.Id,
            Nome = "",
            Email = "ana@nova.com",
            DataNascimento = "11/11/2002",
            Cpf = pessoa.Cpf,
            Sexo = pessoa.Sexo,
            Nacionalidade = pessoa.Nacionalidade,
            Naturalidade = pessoa.Naturalidade,
            Endereco = "Rua Nova"
        };

        // Act
        var resultado = await service.UpdateAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact(DisplayName = "UpdateAsync - Deve falhar ao atualizar dados da pessoa com cpf invalido")]
    public async Task Cenario_14()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.Server.Model.Pessoa.Create(
            "Ana",
            "ana@email.com",
            DateTime.Today.AddYears(-28),
            "111.333.555-66",
            Sexo.Feminino,
            Nacionalidade.Brasileira,
            "BA",
            "Rua D").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        var request = new EditarPessoaRequest
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Email = pessoa.Email,
            DataNascimento = "11/11/2002",
            Cpf = "12341342.245245-2534234",
            Sexo = pessoa.Sexo,
            Nacionalidade = pessoa.Nacionalidade,
            Naturalidade = pessoa.Naturalidade,
            Endereco = pessoa.Endereco
        };

        // Act
        var resultado = await service.UpdateAsync(request);

        // Assert
        Assert.False(resultado.FoiSucesso);
        Assert.Null(resultado.Valor);
    }

    [Fact(DisplayName = "DeleteAsync - Deve deletar pessoa com sucesso")]
    public async Task Cenario_15()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.Server.Model.Pessoa.Create(
            "Ana",
            "ana@email.com",
            DateTime.Today.AddYears(-28),
            "111.333.555-66",
            Sexo.Feminino,
            Nacionalidade.Brasileira,
            "BA",
            "Rua D").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.DeleteAsync(pessoa.Id);

        // Assert
        Assert.True(resultado.FoiSucesso);
        Assert.Equal(pessoa.Id, resultado.Valor);
    }

    [Fact(DisplayName = "DeleteAsync - Deve falhar ao deletar pessoa com id inválido")]
    public async Task Cenario_16()
    {
        // Arrange
        var contexto = GetDbContext();
        var repo = new PessoaRepository(contexto);
        var service = new PessoaService(repo);

        var pessoa = Pessoas.Server.Model.Pessoa.Create(
            "Ana",
            "ana@email.com",
            DateTime.Today.AddYears(-28),
            "111.333.555-66",
            Sexo.Feminino,
            Nacionalidade.Brasileira,
            "BA",
            "Rua D").Valor;

        contexto.Pessoas.Add(pessoa);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.DeleteAsync(Guid.Empty);

        // Assert
        Assert.False(resultado.FoiSucesso);
    }
}
