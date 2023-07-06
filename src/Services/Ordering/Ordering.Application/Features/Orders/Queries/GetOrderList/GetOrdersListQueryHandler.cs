using AutoMapper;
using FluentResults;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrdersListQueryHandler:IRequestHandler<GetOrdersListQuery, Result<List<OrdersVm>>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;


        public GetOrdersListQueryHandler(IMapper mapper,IOrderRepository orderRepository)
        {
            _mapper = mapper ??throw new ArgumentNullException(nameof(mapper));
            _orderRepository = orderRepository  ?? throw new ArgumentNullException(nameof(orderRepository));
        }



        public async Task<Result<List<OrdersVm>>> Handle(GetOrdersListQuery request, 
            CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
            var result = _mapper.Map<List<OrdersVm>>(orderList);
            //return _mapper.Map<List<OrdersVm>>(orderList);

            return Result.Ok(result).WithSuccess("Success");


        }
    }
}
