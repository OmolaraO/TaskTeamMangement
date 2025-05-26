using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using TeamTaskManagement;
using TeamTaskManagement.Data;
using TeamTaskManagement.Services.Interfaces;
using TeamTaskManagement.Services.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.ConfigureApplication();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureSwagger();



builder.Services.AddScoped<IPasswordHasher<TeamTaskManagement.Models.User>, PasswordHasher<TeamTaskManagement.Models.User>>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();   

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
