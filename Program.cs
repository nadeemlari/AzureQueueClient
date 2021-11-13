using Azure.Identity;
using Azure.Storage.Queues;

using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAzureClients(builder =>
{
    builder.AddClient<QueueClient, QueueClientOptions>((_, _, _) =>
    {
        return new QueueClient(new Uri("https://mnlstrg.queue.core.windows.net/que"),new DefaultAzureCredential());
    });


});

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
