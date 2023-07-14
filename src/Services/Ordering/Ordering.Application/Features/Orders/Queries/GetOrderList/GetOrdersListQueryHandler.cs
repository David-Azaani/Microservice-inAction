using AutoMapper;
using FluentResults;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;


        public GetOrdersListQueryHandler(IMapper mapper, IOrderRepository orderRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }



        public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request,
            CancellationToken cancellationToken)
        {
            //try
            //{
            //    var result = new List<OrdersVm>();


            //    var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
            //    if (orderList.Any())
            //    {
            //        result = _mapper.Map<List<OrdersVm>>(orderList);
            //        //return result;

            //    }

            //    return Result.Ok(result).WithSuccess("Success");



            //}
            //catch (Exception e)
            //{
            //    return Result.Fail(new Error("Exception").CausedBy(e.Message));
            //}
            var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
            var result = _mapper.Map<List<OrdersVm>>(orderList);
            return result;



        }
    }
}
