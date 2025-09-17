using OrderBookApi.Repositories;
using OrderBookApi.Services;
using OrderBookApi.Hubs;


var builder = WebApplication.CreateBuilder(args);


// Services
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();


// App services
builder.Services.AddSingleton<IOrderBookProvider, BitstampOrderBookProvider>();
builder.Services.AddSingleton<OrderBookManager>();
// Background poller that fetches snapshots periodically
builder.Services.AddHostedService<OrderBookPollerService>();


// Audit repository (file-based)
builder.Services.AddSingleton<IAuditRepository>(_ => new FileAuditRepository("audit/orderbook-audit.log"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .WithOrigins("http://localhost:5173") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseStaticFiles();
app.UseCors();
app.MapControllers();
app.MapHub<OrderBookHub>("/orderbook");


app.Run();