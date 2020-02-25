using Orleans;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public interface IOrderGrain : IGrainWithGuidKey
    {
        ValueTask PlaceOrderAsync(Order order);
        ValueTask<Order> GetOrderAsync();
    }
}
