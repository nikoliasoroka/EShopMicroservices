using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data;

public class CashedBasketRepository(IBasketRepository repository, IDistributedCache cashe)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cashedBasket = await cashe.GetStringAsync(userName, cancellationToken);

        if (!string.IsNullOrEmpty(cashedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cashedBasket)!;

        var basket = await repository.GetBasket(userName, cancellationToken);
        await cashe.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);
        await cashe.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(userName, cancellationToken);        
        await cashe.RemoveAsync(userName, cancellationToken);

        return true;
    }
}
