using Azure.Storage.Queues;
using MediatrExample.ApplicationCore.Common.Services;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace MediatrExample.ApplicationCore.Infrastructure.Services.AzureQueues;

public class AzureStorageQueueService : IQueuesService
{
    private readonly IConfiguration _config;

    public AzureStorageQueueService(IConfiguration config)
    {
        _config = config;
    }

    public async Task QueueAsync<T>(string queueName, T item)
    {
        var queueClient = new QueueClient(_config.GetConnectionString("StorageAccount"), queueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExists();

        var message = JsonSerializer.Serialize(item);
        await queueClient.SendMessageAsync(message);
    }
}