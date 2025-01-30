using Microsoft.EntityFrameworkCore;
using EventModsen.Infrastructure.DB;
using EventModsen.Domain.Interfaces;
using EventModsen.Infrastructure.DB.Repositories;
using EventModsen.Application.Interfaces;
using EventModsen.Application.Services;
using EventModsen.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EventDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<PaginationSettings>(builder.Configuration.GetSection("PaginationSettings"));

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IMemberService, MemberService>();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
