using FluentEx2.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(option =>
{
    option.Filters.Add<FluentValidationServiceFilter>();
});

//builder.Services.AddValidatorsFromAssemblyContaining<ProductSaveModelValidation>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<ProductSaveModelValidation>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
