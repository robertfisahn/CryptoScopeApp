using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CryptoScopeAPI.Dtos;
using CryptoScopeAPI.Exceptions;

namespace CryptoScopeAPI.Features.GetCoins
{
    public class GetCoinsQueryHandler(AppDbContext _db, IMapper _mapper) : IRequestHandler<GetCoinsQuery, List<CoinListDto>>
    {
        public async Task<List<CoinListDto>> Handle(GetCoinsQuery request, CancellationToken cancellationToken)
        {
            var coins = await _db.Coins
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CoinListDto>>(
                coins.OrderByDescending(c => c.MarketCapUsd).ToList()
            );
        }
    }

}