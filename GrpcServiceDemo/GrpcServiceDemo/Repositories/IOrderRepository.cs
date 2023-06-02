using GrpcServiceDemo.Models;

namespace GrpcServiceDemo.Repositories
{
    public interface IOrderRepository
    {
        public Task<Order> GetOrder();
    }
}
