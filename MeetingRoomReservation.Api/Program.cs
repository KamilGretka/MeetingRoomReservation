using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Profiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetUsersHandler).Assembly));

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserProfile>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();