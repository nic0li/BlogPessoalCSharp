using BlogPessoal.Model;
using BlogPessoal.Security.Implements;
using BlogPessoal.Security;
using BlogPessoal.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogPessoal.Configuration;
using Microsoft.OpenApi.Models;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using BlogPessoal.Service.Interfaces;
using BlogPessoal.Service;
using BlogPessoal.Context;
using BlogPessoal.Repository;
using BlogPessoal.Repository.Interfaces;

namespace BlogPessoal;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        // Add Controller Class
        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
                {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                }
            );

        // Conexão com o Banco de dados
        var connectionString = builder.Configuration.
                GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString)
        );

        // Validação das Entidades
        builder.Services.AddTransient<IValidator<Usuario>, UsuarioValidator>();
        builder.Services.AddTransient<IValidator<Tema>, TemaValidator>();
        builder.Services.AddTransient<IValidator<Publicacao>, PublicacaoValidator>();
        builder.Services.AddTransient<IValidator<Comentario>, ComentarioValidator>();

        // Registrar as Classes e Interfaces Service
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        builder.Services.AddScoped<ITemaService, TemaService>();
        builder.Services.AddScoped<IPublicacaoService, PublicacaoService>();
        builder.Services.AddScoped<IComentarioService, ComentarioService>();

        // Registrar as Classes e Interfaces Repository
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped<ITemaRepository, TemaRepository>();
        builder.Services.AddScoped<IPublicacaoRepository, PublicacaoRepository>();
        builder.Services.AddScoped<IComentarioRepository, ComentarioRepository>();

        // Adicionar a Validação do Token JWT

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var Key = Encoding.UTF8.GetBytes(Settings.Secret);
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
            };
        });

        // Learn more about configuring Swagger/OpenAPI
        // at https://aka.ms/aspnetcore/swashbuckle

        builder.Services.AddEndpointsApiExplorer();

        //Registrar o Swagger
        builder.Services.AddSwaggerGen(options =>
        {

            //Personalizar a Págna inicial do Swagger
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Projeto Blog Pessoal",
                Description = "Projeto Blog Pessoal - ASP.NET Core 7 - Entity Framework",
                Contact = new OpenApiContact
                {
                    Name = "Generation Brasil",
                    Email = "conteudogeneration@generation.org",
                    Url = new Uri("https://github.com/conteudoGeneration")
                },
                License = new OpenApiLicense
                {
                    Name = "Github",
                    Url = new Uri("https://github.com/conteudoGeneration")
                }
            });

            //Adicionar a Segurança no Swagger
            options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Digite um Token JWT válido!",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            //Adicionar a configuração visual da Segurança no Swagger
            options.OperationFilter<AuthResponsesOperationFilter>();

        });

        // Adicionar o Fluent Validation no Swagger
        builder.Services.AddFluentValidationRulesToSwagger();


        // Configuração do CORS
        builder.Services.AddCors(options => {
            options.AddPolicy(name: "MyPolicy",
                policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });

        var app = builder.Build();

        // Criar o Banco de dados e as tabelas Automaticamente
        using (var scope = app.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();

        }

        app.UseDeveloperExceptionPage();

        // Habilitar o Swagger
        app.UseSwagger();

        app.UseSwaggerUI();

        //Habilitar CORS

        app.UseCors("MyPolicy");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Habilitar a Autenticação e a Autorização

        app.UseAuthentication();

        app.UseAuthorization();

        // Habilitar Controller
        app.MapControllers();

        app.Run();
    }
}