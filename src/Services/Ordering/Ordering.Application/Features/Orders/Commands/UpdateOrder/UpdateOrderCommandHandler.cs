﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository,
            IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
               

                _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));

                await _orderRepository.UpdateAsync(orderToUpdate);

                _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated.");

                return Result.Ok().WithSuccess($"Order {orderToUpdate.Id} is successfully updated.");
            }
            catch (Exception e)
            {
                return Result.Fail(new Error("Exception").CausedBy(e.Message));
            }
        }
    }
}