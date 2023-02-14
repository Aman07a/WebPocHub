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

app.MapGet("/api/employee/{id:int}", async (int id, ICommonRepository<Employee> _repository) =>
{
	var employee = await _repository.GetDetails(id);
	return employee == null ? Results.NotFound() : Results.Ok(employee);

}).WithName("Details")
.Produces<Employee>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.MapPost("/api/emloyees", async (Employee employee, ICommonRepository<Employee> _repository) =>
{
	var output = await _repository.Insert(employee);

	if (output != null)
	{
		return Results.Created($"/api/employees/{output.EmployeeId}", output);
	}

	return Results.BadRequest();

}).WithName("Create")
.Produces<Employee>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

app.Run();