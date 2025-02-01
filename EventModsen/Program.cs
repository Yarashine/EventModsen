using Microsoft.EntityFrameworkCore;
using EventModsen.Infrastructure.DB;
using EventModsen.Domain.Interfaces;
using EventModsen.Infrastructure.DB.Repositories;
using EventModsen.Application.Interfaces;
using EventModsen.Application.Services;
using EventModsen.Configuration;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EventDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<PaginationSettings>(builder.Configuration.GetSection("PaginationSettings"));
builder.Services.Configure<ImageSettings>(builder.Configuration.GetSection("ImageSettings"));

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images")),
    RequestPath = "/Images"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
