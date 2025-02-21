using Microsoft.EntityFrameworkCore;
using whatsapp.Application.Mapping;
using whatsapp.Application.Repo;
using whatsapp.Application.Services;
using whatsapp.Domain.RepoContract;
using whatsapp.Domain.RepoContract.recieveMessages;
using whatsapp.Domain.ServiceContract;
using whatsapp.Infastructure.Database;
using whatsapp.Infastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<WhatsappApiContext>();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


builder.Services.AddScoped<IPhoneNumRepo, PhoneNumRepo>();
builder.Services.AddScoped<IGetPhoneNumService, GetPhoneNumService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IRecieveMessageService, RecievedMessageService>();
builder.Services.AddScoped<IRecieveMessageRepo, RecieveMessageRepo>();

builder.WebHost.UseUrls("http://0.0.0.0:7284");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .WithOrigins("http://localhost:4200")  // Allow only Angular app
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // Important for SignalR with authentication
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();
app.UseRouting();

app.UseCors("AllowAngularApp");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<WaHub>("/waapiHub").RequireCors("AllowAngularApp"); // Apply CORS to SignalR
});
app.UseAuthorization();


app.Run();

