using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CryptoScopeAPI.Dtos;

namespace CryptoScopeAPI.Features.GetSearchCoin
{
    public class GetSearchCoinsQueryHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetSearchCoinQuery, List<SearchCoinDto>>
    {
        public async Task<List<SearchCoinDto>> Handle(GetSearchCoinQuery request, CancellationToken cancellationToken)
        {
            var coins = await _db.SearchCoins
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<SearchCoinDto>>(coins);
        }
    }
}
