using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand,Result>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;


        public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<DeleteOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
                //if (orderToDelete == null)
                //{
                //    throw new NotFoundException(nameof(Order), request.Id);
                //}
               
                await _orderRepository.DeleteAsync(orderToDelete);

                _logger.LogInformation($"Order {orderToDelete.Id} is successfully deleted.");

                return Result.Ok().WithSuccess($"Order {orderToDelete.Id} is successfully deleted.");
            }
            catch (Exception e)
            {
                return Result.Fail(new Error("Exception").CausedBy(e.Message));
            }
        }
    }
}
