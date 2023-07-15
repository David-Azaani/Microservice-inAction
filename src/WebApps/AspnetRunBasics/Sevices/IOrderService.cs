using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.Model;

namespace AspnetRunBasics.Sevices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
    }

}