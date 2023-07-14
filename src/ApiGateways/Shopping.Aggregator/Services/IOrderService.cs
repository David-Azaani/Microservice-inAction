
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public interface IOrderService
{
     Task<List<OrderResponseModel>> GetOrdersByUserName(string userName);
}