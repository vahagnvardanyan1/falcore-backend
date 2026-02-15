using VTS.DAL;
using VTS.BLL;
using VTS.API.Hubs;
using VTS.API.Extensions;
using VTS.API.Middlewares;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddTrackerContext(builder.Configuration);

builder.Services.AddAutoMapper();
builder.Services.AddFluentValidation();
builder.Services.AddTrackerServices(builder.Configuration);
builder.Services.AddNotificationServices();
builder.Services.SetupJobs(builder.Configuration);

var app = builder.Build();

app.MapSwaggerDocumentation();
app.UseCors(policy => policy.SetIsOriginAllowed(_ => true)
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .AllowCredentials());

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.Run();
