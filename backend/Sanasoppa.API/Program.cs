using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Extensions;
using Sanasoppa.API.Hubs;
using Sanasoppa.Model.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<SanasoppaContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("SanasoppaDb"));
});

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Environment, builder.Configuration);
builder.Services.AddConfigurationsServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("hubs/gamehub");

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
