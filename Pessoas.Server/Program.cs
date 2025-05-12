using Pessoas.Server.Infra;
using Pessoas.Server.Infra.Extensoes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilita arquivos estáticos do SPA
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "../pessoas.client/dist";
});

// Instalar serviços via padrão Installer
builder.Services.InstallServicesFromAssembly(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    Seeding.SeedPessoas(db);
    Seeding.SeedUsuarios(db);
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSpaStaticFiles(); // Agora vai funcionar sem erro

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pessoas API V1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Pessoas API V2");
    options.DocumentTitle = "Pessoas.API Docs";
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Fallback para SPA
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "../pessoas.client"; // usado apenas no dev
});

app.MapFallbackToFile("/index.html");

app.Run();
