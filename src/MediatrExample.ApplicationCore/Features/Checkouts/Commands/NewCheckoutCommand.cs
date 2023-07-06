using FluentValidation.Results;
using MediatR;
using MediatrExample.ApplicationCore.Common.Attributes;
using MediatrExample.ApplicationCore.Common.Exceptions;
using MediatrExample.ApplicationCore.Common.Helpers;
using MediatrExample.ApplicationCore.Common.Interfaces;
using MediatrExample.ApplicationCore.Common.Messages;
using MediatrExample.ApplicationCore.Common.Services;
using MediatrExample.ApplicationCore.Domain;
using MediatrExample.ApplicationCore.Infrastructure.Persistence;

namespace MediatrExample.ApplicationCore.Features.Checkouts.Commands;

[AuditLog]
public class NewCheckoutCommand : IRequest
{
    public List<NewCheckoutItems> Products { get; set; } = new();

    public class NewCheckoutItems
    {
        public string ProductId { get; set; } = default!;
        public int Quantity { get; set; }
    }
}

public class NewCheckoutCommandHandler : IRequestHandler<NewCheckoutCommand>
{
    private readonly MyAppDbContext _context;
    private readonly IQueuesService _queuesService;
    private readonly CurrentUser _user;

    public NewCheckoutCommandHandler(MyAppDbContext context, ICurrentUserService currentUserService, IQueuesService queuesService)
    {
        _context = context;
        _queuesService = queuesService;
        _user = currentUserService.User;
    }

    public async Task Handle(NewCheckoutCommand request, CancellationToken cancellationToken)
    {
        Checkout newCheckout = await CreateCheckoutAsync(request, cancellationToken);

        await QueueCheckout(newCheckout);
    }

    private async Task<Checkout> CreateCheckoutAsync(NewCheckoutCommand request, CancellationToken cancellationToken)
    {
        var newCheckout = new Checkout
        {
            CheckoutDateTime = DateTime.UtcNow,
            UserId = _user.Id,
            Total = 0
        };

        foreach (var item in request.Products)
        {
            var product = await _context.Products.FindAsync(item.ProductId.FromHashId());

            if (product is null)
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("Error", $"El producto {item.ProductId} no existe")
                });
            }

            var newProduct = new CheckoutProduct
            {
                ProductId = product.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                Total = item.Quantity * product.Price
            };

            newCheckout.Products.Add(newProduct);
        }

        newCheckout.Total = newCheckout.Products.Sum(p => p.Total);

        _context.Checkouts.Add(newCheckout);

        await _context.SaveChangesAsync(cancellationToken);
        return newCheckout;
    }

    private async Task QueueCheckout(Checkout newCheckout)
    {
        await _queuesService.QueueAsync("new-checkouts", new NewCheckoutMessage
        {
            CheckoutId = newCheckout.CheckoutId
        });
    }
}