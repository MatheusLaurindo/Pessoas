using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Pessoas.Server.Infra;
using Pessoas.Server.Services;

namespace Tests.ServiceTests.Auth;

public class AuthServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        Seeding.SeedUsuarios(context);

        return context;
    }

    private IConfiguration GetMockConfiguration()
    {
        var config = new Dictionary<string, string>
        {
            { "JWT:ISSUER", "TestIssuer" },
            { "JWT:AUDIENCE", "TestAudience" },
            { "JWT:KEY", "a74a7232f16847d7068a003dd66b55323fce51c9a0fd7382ceab438c5df24a566044e767088b6dc2da6d5670dbff24f0061b2f8ceb7ac9976c39f348d67a1001816e51ba5b43e6e596e17daf8ebff6ec6e5d991cfa0556480eb918b2319c1a61d62c0b1f702229b6242e87f1647a93d92175129c137445ccf960f844e20f8286" }
        };

        return new ConfigurationBuilder().AddInMemoryCollection(config).Build();
    }

    [Fact(DisplayName = "Authenticate - Deve retonar retornar token se credencais validas")]
    public async Task Cenario_1()
    {
        // Arrange
        var context = GetDbContext();
        var service = new AuthService(context, GetMockConfiguration());

        // Act
        var result = await service.Authenticate("admin@gmail.com", "admin123");

        // Assert
        Assert.True(result.FoiSucesso);
        Assert.NotNull(result.Valor);
        Assert.False(string.IsNullOrEmpty(result.Valor.JWT_TOKEN));
    }

    [Fact(DisplayName = "Authenticate - Deve falhar se usuario não for encontrado")]
    public async Task Cenario_2()
    {
        // Arrange
        var context = GetDbContext();
        var service = new AuthService(context, GetMockConfiguration());

        // Act
        var result = await service.Authenticate("inexistente@example.com", "inexistente");

        // Assert
        Assert.False(result.FoiSucesso);
        Assert.Null(result.Valor);
    }

    [Fact(DisplayName = "Authenticate - Deve falhar configuracoes não encontradas (issuer, audience e key)")]
    public async Task Cenario_3()
    {
        // Arrange
        var context = GetDbContext();
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(x => x["JWT:ISSUER"]).Returns("issuer");
        mockConfig.Setup(x => x["JWT:AUDIENCE"]).Returns("audience");
        mockConfig.Setup(x => x["JWT:KEY"]).Returns("short");

        var service = new AuthService(context, mockConfig.Object);

        // Act
        var result = await service.Authenticate("inexistente@example.com", "inexistente");

        // Assert
        Assert.False(result.FoiSucesso);
        Assert.Null(result.Valor);
    }
}
