using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;
using Shared.Contracts;

namespace BackEnd.Services
{
    public class OrderGrain : Grain, IOrderGrain
    {
        private readonly IPersistentState<Order> _state;

        public OrderGrain([PersistentState("order")] IPersistentState<Order> state)
        {
            _state = state;
        }

        public ValueTask PlaceOrderAsync(Order order)
        {
            _state.State = order;
            return new ValueTask(_state.WriteStateAsync());
        }

        public ValueTask<Order> GetOrderAsync() => new ValueTask<Order>(_state.State);
    }
}
