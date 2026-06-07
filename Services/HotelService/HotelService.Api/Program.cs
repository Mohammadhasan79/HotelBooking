using FluentValidation.AspNetCore;
using FluentValidation;
using HotelService.Api.Controllers;
using HotelService.Application.Mappings;
using HotelService.Application.Validators;
using HotelService.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAutoMapper(typeof(HotelProfile));

builder.Services.AddControllers()
    .AddApplicationPart(typeof(HotelsController).Assembly);
builder.Services
    .AddFluentValidationAutoValidation();

builder.Services
    .AddValidatorsFromAssemblyContaining<
        CreateHotelDtoValidator>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

