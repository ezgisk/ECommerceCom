using ECommerceCom.Business.Types;
using ECommerceCom.Data.Entities;
using ECommerceCom.Data.Repositories;
using ECommerceCom.Data.UnitOfWork;
using ECommerceCom.Business.Operations.Order.Dtos; // Ensure you include the correct namespace for DTOs
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Order
{
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IRepository<OrderProductEntity> _orderProductRepository;

        public OrderManager(IUnitOfWork unitOfWork, IRepository<OrderEntity> orderRepository,
            IRepository<OrderProductEntity> orderProductRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
        }

        public async Task<ServiceMessage> AddOrder(AddOrderDto order)
        {
            // Check if an order already exists for the same customer on the same date
            var hasOrder = _orderRepository.GetAll(x => x.CustomerId == order.CustomerId && x.OrderDate.Date == order.OrderDate.Date).Any();
            if (hasOrder)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu müşteri için bu tarihte bir sipariş zaten mevcut."
                };
            }

            // Begin transaction for creating the order and adding products
            await _unitOfWork.BeginTransaction();

            var orderEntity = new OrderEntity
            {
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                CustomerId = order.CustomerId
            };

            _orderRepository.Add(orderEntity); // Add the order to the repository

            try
            {
                await _unitOfWork.SaveChangesAsync(); // Save the order entity first

                // Now add the products to the order
                foreach (var orderProduct in order.OrderProducts)
                {
                    var orderProductEntity = new OrderProductEntity
                    {
                        OrderId = orderEntity.Id, // Link the order product to the created order
                        ProductId = orderProduct.ProductId,
                        Quantity = orderProduct.Quantity
                    };

                    _orderProductRepository.Add(orderProductEntity); // Add each product to the repository
                }

                // Save the order products
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction(); // Commit the transaction if everything is successful
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransaction(); // Rollback if anything goes wrong
                throw new Exception($"Order kaydı sırasında bir hata oluştu, süreç başa sarıldı: {ex.Message}", ex);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Sipariş başarıyla oluşturuldu."
            };
        }
    }
}
