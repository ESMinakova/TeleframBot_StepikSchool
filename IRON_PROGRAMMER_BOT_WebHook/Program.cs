using IRON_PROGRAMMER_BOT_Common;
using IRON_PROGRAMMER_BOT_WebHook;

var builder = WebApplication.CreateBuilder(args); 

// Add services to the container.
ContainerConfigurator.Configure(builder.Configuration, builder.Services);

builder.Services.AddHostedService<WebhookConfigurator>();


builder.Services.AddControllers()
        .AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
