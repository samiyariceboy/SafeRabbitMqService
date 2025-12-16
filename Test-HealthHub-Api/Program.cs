using RabbitMQ.Client;
using Test_HealthHub_Api;
using Microsoft.Extensions.Options;
using Taday.Corelibrary.Common.Shared;
using Taday.Corelibrary.Infrastucture.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOptions<RabbitMqOption>()
    .Bind(builder.Configuration.GetSection("RabbitMq"))
    .ValidateOnStart();


builder.Services.AddSingleton(provider =>
{
    var rabbitMq = provider.GetRequiredService<IOptions<RabbitMqOption>>().Value;
    
    return new ConnectionFactory()
    {
        UserName = rabbitMq.UserName,
        Password = rabbitMq.Password,
        HostName = rabbitMq.HostName,
        VirtualHost = "/",
        Port = rabbitMq.Port,
        DispatchConsumersAsync = rabbitMq.DispatchConsumersAsync

        //Ssl = new SslOption
        //{
        //    Enabled = rabbitMq.Ssl,
        //},  
    };
});

builder.Services.AddSingleton<PublisherState>();

builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(Taday.Corelibrary.Common.Shared.Logger<>));

builder.Services.AddHostedService<PublisherService>();

builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>(provider => 
{
    var factory = provider.GetRequiredService<ConnectionFactory>();
    var appSetting = provider.GetRequiredService<IOptions<RabbitMqOption>>();
    var logger = provider.GetRequiredService<IAppLogger<RabbitMqBaseService>>();

    return new RabbitMqService(logger, factory, appSetting.Value);
});

builder.Services.AddOpenApi();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();

