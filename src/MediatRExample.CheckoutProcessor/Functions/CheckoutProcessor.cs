using MediatR;
using MediatrExample.ApplicationCore.Common.Messages;
using MediatrExample.ApplicationCore.Features.Checkouts.Commands;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediatRExample.CheckoutProcessor.Functions
{
    public class CheckoutProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public CheckoutProcessor(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<CheckoutProcessor>();
            _mediator = mediator;
        }

        [Function("CheckoutProcessor")]
        public async Task Run([QueueTrigger("new-checkouts", Connection = "AzureWebJobsStorage")] string myQueueItem)
        {
            _logger.LogInformation("Nuevo mensaje recibido {Message}", myQueueItem);
            _logger.LogInformation("Procesando...");

            var message = JsonSerializer.Deserialize<NewCheckoutMessage>(myQueueItem);

            await _mediator.Send(new ProcessCheckoutCommand
            {
                CheckoutId = message.CheckoutId
            });
        }
    }
}
