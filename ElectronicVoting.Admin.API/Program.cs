using System.Reflection;
using ElectronicVoting.Admin.Application.Services;
using Microsoft.AspNetCore.Builder;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using ElectronicVoting.Admin.Infrastructure.Exceptions;
using ElectronicVoting.Admin.Infrastructure.JwtBearer;
using ElectronicVoting.Admin.Infrastructure.Lirisi;
using ElectronicVoting.Admin.Infrastructure.MediatR;
using ElectronicVoting.Admin.Infrastructure.Paillier;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
var applicationAssembly = typeof(ElectronicVoting.Admin.Application.AssemblyReference)?.Assembly;
// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("Authorization");
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddWrappers();
builder.Services.AddPaillier();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddGlobalException();
builder.Services.AddMediatR(applicationAssembly);
builder.Services.AddJwtBearer(builder.Configuration);
builder.Services.AddEntityFramework(builder.Configuration);
builder.Services.AddValidatorsFromAssembly(applicationAssembly);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ElectionDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseExceptionHandler();
app.UseMiniProfiler();
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
