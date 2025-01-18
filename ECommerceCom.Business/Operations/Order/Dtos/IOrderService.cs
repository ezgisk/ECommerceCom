using ECommerceCom.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Order.Dtos
{
    public interface IOrderService
    {
        Task<ServiceMessage> AddOrder(AddOrderDto order);
        Task<OrderDetailsDto> GetOrder(int orderId);
        Task<List<OrderDetailsDto>> GetOrders();
        Task<ServiceMessage> AdjustOrderTotalAmount(int id, decimal changeTo);
        Task<ServiceMessage> DeleteOrder(int id);
        Task<ServiceMessage> UpdateOrder(UpdateOrderDto order);
    }
}
