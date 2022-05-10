using MediatrExample.ApplicationCore.Common.Interfaces;
using System;

namespace MediatRExample.CheckoutProcessor;
public class WorkerUserService : ICurrentUserService
{
    public CurrentUser User => new CurrentUser(Guid.Empty.ToString(), "CheckoutProcessor", true);

    public bool IsInRole(string roleName) => true;
}
