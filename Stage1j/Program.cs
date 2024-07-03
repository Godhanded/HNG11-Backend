
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Stage1.Models;
using Stage1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ForwardedHeadersOptions>(op=>{
    op.ForwardedHeaders= ForwardedHeaders.XForwardedFor |ForwardedHeaders.XForwardedProto;
});
builder.Services.AddScoped<HttpService>(x=>new HttpService(new HttpClient(),builder.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/api/hello",async (HttpService client,HttpContext context,[FromQuery]string? visitor_name)=>
{
    try{
    var forwardedFor = context.GetServerVariable("HTTP_X_FORWARDED_FOR");
    var clientIp=forwardedFor?.Split(',').FirstOrDefault()?.Trim();
    if (string.IsNullOrEmpty(clientIp))
        clientIp = context.Connection.RemoteIpAddress?.ToString();
    if (clientIp is null)
        return Results.NotFound("Client Ip Not Found");

    // api check
    
    (string? city,double? temperature)=await client.GetIpDetails(clientIp);
    if ((city is null) || (temperature is null))
        return Results.NotFound("Ip Details Was Not Found");
    

    var response = new ResponseDto(clientIp, city, $"Hello, {visitor_name??"Anonymous"}!, the temperature is {temperature} degrees Celcius in {city}");
    return Results.Ok(response);
    }catch(Exception e){
        return Results.Problem(e.ToString());
    }
}).WithName("GetIpInfo");

app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}