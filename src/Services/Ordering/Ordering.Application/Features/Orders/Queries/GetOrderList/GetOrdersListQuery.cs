using FluentResults;
using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrdersListQuery:IRequest<Result<List<OrdersVm>>>
    {
        public string UserName { get; set; }

        public GetOrdersListQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
