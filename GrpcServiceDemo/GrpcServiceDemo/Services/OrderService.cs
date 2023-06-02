using Grpc.Core;
using GrpcServiceDemo;
using GrpcServiceDemo.Repositories;
namespace GrpcServiceDemo.Services
{
    public class OrderService : OrderProcessing.OrderProcessingBase
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderRepository _orderRepository;
        public OrderService(ILogger<OrderService> logger,
          IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public override Task<OrderResponse> GetOrder
          (OrderRequest request, ServerCallContext context)
        {
            return Task.FromResult(new OrderResponse
            {
                Order = _orderRepository.GetOrder().Result
            });
        }
    }
}
