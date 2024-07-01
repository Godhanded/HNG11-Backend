using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stage1.Models;
using Stage1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<HttpService>(x=>new HttpService(new HttpClient(),builder.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/api/hello",async (HttpService client,HttpContext context,[FromQuery]string visitor_name="Anonymouse")=>
{
    var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var clientIp=forwardedFor?.Split(',').FirstOrDefault()?.Trim();
    if (string.IsNullOrEmpty(clientIp))
        clientIp = context.Connection.RemoteIpAddress?.ToString();
    if (clientIp is null)
        return Results.NotFound("Client Ip Not Found");
    // api check
    
    (string? city,string? temperature)=await client.GetIpDetails(clientIp);
    if ((city is null) || (temperature is null))
        return Results.NotFound("Ip Details Was Not Found");
    

    var response = new ResponseDto(clientIp, city, $"Hello, {visitor_name} the temperature is{temperature} degrees Celcius in New York");

    return Results.Ok(response);
}).WithName("GetIpInfo");
app.Run();

