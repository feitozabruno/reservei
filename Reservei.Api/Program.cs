using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Reservei.Core.Entities;
using Reservei.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

// ==========================================================
// 1. CONFIGURAÇÃO DO BANCO DE DADOS
// ==========================================================

// Pega a string de conexão do appsettings.Development.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configura o Entity Framework (Para Escrita e Migrations)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configura o Dapper (Para Leitura Rápida)
// Injetamos IDbConnection para poder usar querys SQL puras quando precisar
builder.Services.AddScoped<IDbConnection>(sp => 
    new NpgsqlConnection(connectionString));

// ==========================================================
// 2. CONFIGURAÇÃO DA AUTENTICAÇÃO (IDENTITY)
// ==========================================================
builder.Services.AddAuthorization();

// Adiciona os endpoints de API do Identity e conecta com nosso AppDbContext
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<AppDbContext>();

// ==========================================================
// 3. CONFIGURAÇÃO DO SWAGGER (Documentação Visual)
// ==========================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================================================
// 4. PIPELINE DE EXECUÇÃO (Como a API responde)
// ==========================================================

// Se estiver em modo de desenvolvimento, ativa o Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapeia as rotas prontas de autenticação (/register, /login, etc.)
app.MapIdentityApi<User>();

app.Run();