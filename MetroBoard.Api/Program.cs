using MetroBoard.Api.Service;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddSingleton<ArrivalsService>();
builder.Services.AddSingleton<ScreenService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<ScreenService>());
builder.Services.AddSingleton<StationService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<StationService>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
