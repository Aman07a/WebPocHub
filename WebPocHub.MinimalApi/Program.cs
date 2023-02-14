using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using WebPocHub.Dal;
using WebPocHub.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WebPocHubDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConStr"));
});

builder.Services.AddScoped<ICommonRepository<Employee>, CommonRepository<Employee>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/employees", async (ICommonRepository<Employee> _repository) =>
{
	var employees = await _repository.GetAll();

	if (employees.Count <= 0)
	{
		return Results.NotFound();
	}

	return Results.Ok(employees);
}).WithName("GetAll")
.Produces<IEnumerable<Employee>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.Run();