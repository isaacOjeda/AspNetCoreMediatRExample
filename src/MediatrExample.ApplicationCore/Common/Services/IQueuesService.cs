namespace MediatrExample.ApplicationCore.Common.Services;
public interface IQueuesService
{
    Task QueueAsync<T>(string queueName, T item);
}
