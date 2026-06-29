var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Swagger aggregator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors("AllowAll");


app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/identity-swagger/swagger/v1/swagger.json", "Identity Service");
    options.SwaggerEndpoint("/hotel-swagger/swagger/v1/swagger.json", "Hotel Service");
    options.SwaggerEndpoint("/room-swagger/swagger/v1/swagger.json", "Room Service");
    options.SwaggerEndpoint("/booking-swagger/swagger/v1/swagger.json", "Booking Service");
    options.SwaggerEndpoint("/payment-swagger/swagger/v1/swagger.json", "Payment Service");
    options.RoutePrefix = "swagger";
});

app.MapReverseProxy();
app.Run();