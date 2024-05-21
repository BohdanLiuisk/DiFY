using System.Text.Json;
using Dify.Entity.Descriptor;
using Dify.Entity.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DifyDb") ?? string.Empty;
builder.Services.AddDifyEntity(options => 
{
    options.ConnectionString = connectionString;
    options.ConfigureDbContext = contextBuilder =>
    {
        contextBuilder.UseNpgsql(connectionString, dbBuilder => 
                dbBuilder.MigrationsAssembly(typeof(Program).Assembly.FullName))
            .UseSnakeCaseNamingConvention();
    };
    options.LoadTablesFromOuterStore = async () =>
    {
        var configsPath = Path.Combine(builder.Environment.ContentRootPath, "entities");
        var fullPath = Path.GetFullPath(configsPath);
        var tables = new List<TableDescriptor>();
        if (!Directory.Exists(fullPath)) return tables;
        foreach (var file in Directory.GetFiles(fullPath, "*.json"))
        {
            var jsonData = await File.ReadAllTextAsync(file);
            var tableDescriptor = TableDescriptor.DeserializeFrom(jsonData);
            if (tableDescriptor != null)
            {
                tables.Add(tableDescriptor);
            }
        }
        return tables;
    };
});

var app = builder.Build();
app.UseDifyEntity();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();
