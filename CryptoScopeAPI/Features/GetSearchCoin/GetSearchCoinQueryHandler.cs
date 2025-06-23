using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CryptoScopeAPI.Dtos;
using CryptoScopeAPI.Exceptions;

namespace CryptoScopeAPI.Features.GetSearchCoin
{
    public class GetSearchCoinsQueryHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetSearchCoinQuery, List<SearchCoinDto>>
    {
        public async Task<List<SearchCoinDto>> Handle(GetSearchCoinQuery request, CancellationToken cancellationToken)
        {
            var coins = await _db.SearchCoins
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);

            if (coins.Count == 0)
            {
                throw new NotFoundException("No searchable coins found.");
            }

            return _mapper.Map<List<SearchCoinDto>>(coins);
        }
    }
}
