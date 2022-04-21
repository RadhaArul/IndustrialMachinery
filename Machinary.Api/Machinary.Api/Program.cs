using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Machinary.Api.Data;
using Machinary.Api.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MachinaryApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MachinaryApiContext")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMachineRepository, MachineRepository > ();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
