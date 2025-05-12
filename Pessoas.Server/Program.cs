using Pessoas.Server.Infra;
using Pessoas.Server.Infra.Extensoes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Instalar serviços via padrão Installer
builder.Services.InstallServicesFromAssembly(builder.Configuration);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pessoas API V1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Pessoas API V2");
    options.DocumentTitle = "Pessoas.API Docs";
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    Seeding.SeedPessoas(db);
    Seeding.SeedUsuarios(db);
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapFallbackToFile("/index.html");

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
