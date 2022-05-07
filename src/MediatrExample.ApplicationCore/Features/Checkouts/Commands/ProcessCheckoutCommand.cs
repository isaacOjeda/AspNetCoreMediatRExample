using MediatR;
using MediatrExample.ApplicationCore.Common.Attributes;
using MediatrExample.ApplicationCore.Common.Exceptions;
using MediatrExample.ApplicationCore.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace MediatrExample.ApplicationCore.Features.Checkouts.Commands;

[AuditLog]
public class ProcessCheckoutCommand : IRequest
{
    public int CheckoutId { get; set; }
}


public class ProcessCheckoutCommandHandler : IRequestHandler<ProcessCheckoutCommand>
{
    private readonly MyAppDbContext _context;
    private readonly ILogger<ProcessCheckoutCommandHandler> _logger;

    public ProcessCheckoutCommandHandler(MyAppDbContext context, ILogger<ProcessCheckoutCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(ProcessCheckoutCommand request, CancellationToken cancellationToken)
    {
        var checkout = await _context.Checkouts.FindAsync(request.CheckoutId);

        if (checkout is null)
        {
            throw new NotFoundException();
        }

        _logger.LogInformation("Nueva orden recibida con Id {Id}", checkout.CheckoutId);
        _logger.LogInformation("El usuario {UserId} ordenó {ProductCount} producto(s)", checkout.UserId, checkout.Products.Count());


        // Working
        _logger.LogInformation("Realizando cobro...");
        await Task.Delay(5000);
        _logger.LogInformation("Cobro realizado");

        checkout.Processed = true;
        checkout.ProcessedDateTime = DateTime.UtcNow;

        _logger.LogWarning("Se procesó una orden con costo total de {Total:C}", checkout.Total);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}