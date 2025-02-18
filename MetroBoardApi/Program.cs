using MetroBoardApi.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSingleton<ScreenService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<ScreenService>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
