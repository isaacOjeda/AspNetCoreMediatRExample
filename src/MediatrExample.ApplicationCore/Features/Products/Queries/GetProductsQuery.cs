using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using MediatrExample.ApplicationCore.Common.Helpers;
using MediatrExample.ApplicationCore.Domain;
using MediatrExample.ApplicationCore.Infrastructure.Persistence;
using MediatrExample.ApplicationCore.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using MediatrExample.ApplicationCore.Common.Models;

namespace MediatrExample.ApplicationCore.Features.Products.Queries;

public class GetProductsQuery : IRequest<PagedResult<GetProductsQueryResponse>>
{
    public string? SortDir { get; set; }
    public string? SortProperty { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<GetProductsQueryResponse>>
{
    private readonly MyAppDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(MyAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<PagedResult<GetProductsQueryResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken) =>
        _context.Products
            .AsNoTracking()
            .OrderBy($"{request.SortProperty} {request.SortDir}")
            .ProjectTo<GetProductsQueryResponse>(_mapper.ConfigurationProvider)
            .GetPagedResultAsync(request.PageSize, request.CurrentPage);
}

public class GetProductsQueryResponse
{
    public string ProductId { get; set; } = default!;
    public string Description { get; set; } = default!;
    public double Price { get; set; }
    public string ListDescription { get; set; } = default!;
}

public class GetProductsQueryProfile : Profile
{
    public GetProductsQueryProfile() =>
        CreateMap<Product, GetProductsQueryResponse>()
            .ForMember(dest =>
                dest.ListDescription,
                opt => opt.MapFrom(mf => $"{mf.Description} - {mf.Price:c}"))
            .ForMember(dest =>
                dest.ProductId,
                opt => opt.MapFrom(mf => mf.ProductId.ToHashId()));

}
