using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Discount.Grpc.Protos;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
//IWebHostEnvironment environment = builder.Environment;
// Add services to the container.
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString");

});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(

    o => o.Address = new Uri(configuration["GrpcSettings:DiscountUrl"])

    );

builder.Services.AddScoped<DiscountGrpcService>();

// MassTransit - RabbitMQ Configuration
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        // configuration["EventBusSettings:HostAddress"]
        cfg.Host( configuration["EventBusSettings:HostAddress"]);
    });

});
//builder.Services.AddMassTransitHostedService();








// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
