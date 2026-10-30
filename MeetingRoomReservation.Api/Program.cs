using FluentValidation;
using MeetingRoomReservation.Api.Database;
using MeetingRoomReservation.Api.Exceptions;
using MeetingRoomReservation.Api.Handlers.User;
using MeetingRoomReservation.Api.Profiles;
using MeetingRoomReservation.Api.Validation;
using MeetingRoomReservation.Api.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{ 
    cfg.RegisterServicesFromAssembly(typeof(GetUsersHandler).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserProfile>();
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();

var app = builder.Build();

app.UseExceptionHandler();

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