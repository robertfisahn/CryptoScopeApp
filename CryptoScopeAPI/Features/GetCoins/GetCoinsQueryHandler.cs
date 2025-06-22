using MediatR;
using Microsoft.EntityFrameworkCore;
using CryptoScopeAPI.Dtos;
using CryptoScopeAPI.Services;
using AutoMapper;
using CryptoScopeAPI.Models;

namespace CryptoScopeAPI.Features.GetCoins
{
    public class GetCoinsQueryHandler(AppDbContext _db, ICoinGeckoClient _client, IMapper _mapper)
    : IRequestHandler<GetCoinsQuery, List<CoinListDto>>
    {
        public async Task<List<CoinListDto>> Handle(GetCoinsQuery request, CancellationToken cancellationToken)
        {
            if (!await _db.Coins.AnyAsync(cancellationToken))
            {
                var fetched = await _client.GetTopMarketCoinsAsync();
                _db.Coins.AddRange(_mapper.Map<Coin>(fetched));
                await _db.SaveChangesAsync(cancellationToken);
            }

            var coins = await _db.Coins.ToListAsync(cancellationToken);

            var sorted = coins
                .OrderByDescending(c => c.MarketCapUsd)
                .ToList();

            return _mapper.Map<List<CoinListDto>>(coins);
        }
    }

}