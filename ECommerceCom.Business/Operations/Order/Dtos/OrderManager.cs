using ECommerceCom.Business.Types;
using ECommerceCom.Data.Entities;
using ECommerceCom.Data.Repositories;
using ECommerceCom.Data.UnitOfWork;
using ECommerceCom.Business.Operations.Order.Dtos; // Ensure you include the correct namespace for DTOs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCom.Business.Operations.Order
{
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IRepository<OrderProductEntity> _orderProductRepository;
        private readonly IRepository<ProductEntity> _productRepository; // Add product repository for product details

        public OrderManager(IUnitOfWork unitOfWork, IRepository<OrderEntity> orderRepository,
            IRepository<OrderProductEntity> orderProductRepository, IRepository<ProductEntity> productRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _productRepository = productRepository;
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

        public async Task<ServiceMessage> AdjustOrderTotalAmount(int id, decimal changeTo)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu id ile eslesen order bulanamadi."
                };
            order.TotalAmount = changeTo;
            _orderRepository.Update(order);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Order degistirilirken bir hata oluştu: {ex.Message}", ex);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Order başarıyla oluşturuldu."
            };

        }

        public async Task<ServiceMessage> DeleteOrder(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu id ile eslesen order bulanamadi."
                };
            }
            _orderRepository.Delete(id);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Order silinirken bir hata oluştu: {ex.Message}", ex);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Order başarıyla silindi."
            };
        }

        public async Task<List<OrderDetailsDto>> GetOrders()
        {
            var orders = await _orderRepository.GetAll().
                 Select(x => new OrderDetailsDto
                 {
                     Id = x.Id,  // Set the order ID
                     OrderDate = x.OrderDate,  // Set the order date from the order entity
                     TotalAmount = x.TotalAmount,  // Set the total amount from the order entity
                     CustomerId = x.CustomerId,  // Safely access customer name, handle null
                                                 // You can add any other fields that belong to the order entity here
                     OrderProducts = x.OrderProducts.Select(op => new OrderProductDto
                     {
                         ProductId = op.ProductId,  // Map product ID
                         Quantity = op.Quantity,    // Map quantity of the ordered product
                         UnitPrice = op.Product != null ? op.Product.Price : 0, // Safely access product price, handle null
                         ProductName = op.Product != null ? op.Product.ProductName : "Unknown" // Safely access product name, handle null
                     }).ToList()  // Collect all order products in a list
                 })
            .ToListAsync();
            return orders;

        }

        public async Task<OrderDetailsDto> GetOrder(int orderId)
        {
            var order = await _orderRepository.GetAll(x => x.Id == orderId).
                 Select(x => new OrderDetailsDto
                 {
                     Id = x.Id,  // Set the order ID
                     OrderDate = x.OrderDate,  // Set the order date from the order entity
                     TotalAmount = x.TotalAmount,  // Set the total amount from the order entity
                     CustomerName = x.Customer != null ? x.Customer.FirstName + x.Customer.LastName : "Unknown",  // Safely access customer name, handle null
                                                                                                                  // You can add any other fields that belong to the order entity here
                     OrderProducts = x.OrderProducts.Select(op => new OrderProductDto
                     {
                         ProductId = op.ProductId,  // Map product ID
                         Quantity = op.Quantity,    // Map quantity of the ordered product
                         UnitPrice = op.Product != null ? op.Product.Price : 0, // Safely access product price, handle null
                         ProductName = op.Product != null ? op.Product.ProductName : "Unknown" // Safely access product name, handle null
                     }).ToList()  // Collect all order products in a list
                 })
        .FirstOrDefaultAsync();

            return order;
        }

        public async Task<ServiceMessage> UpdateOrder(UpdateOrderDto order)
        {
            var orderEntity = _orderRepository.GetById(order.OrderId);
            if (orderEntity == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Order bulunamadı."
                };
            }

            await _unitOfWork.BeginTransaction();
            try
            {
                // Sipariş bilgilerini güncelleme
                if (order.TotalAmount.HasValue)
                {
                    orderEntity.TotalAmount = order.TotalAmount.Value;
                }

                if (order.CustomerId.HasValue)
                {
                    orderEntity.CustomerId = order.CustomerId.Value;
                }

                // Siparişi güncelliyoruz
                _orderRepository.Update(orderEntity);

                // Eski ürünleri ilişki tablosundan (OrderProduct) siliyoruz
                var existingOrderProducts = _orderProductRepository.GetAll(x => x.OrderId == orderEntity.Id).ToList();
                foreach (var product in existingOrderProducts)
                {
                    _orderProductRepository.Delete(product);
                }

                // Yeni ürünleri ekliyoruz
                foreach (var productId in order.OrderProductIds)
                {
                    var orderProduct = new OrderProductEntity
                    {
                        OrderId = orderEntity.Id,
                        ProductId = productId
                    };

                    // OrderProduct zaten varsa güncelle
                    var existingOrderProduct = orderEntity.OrderProducts.FirstOrDefault(op => op.ProductId == productId);
                    if (existingOrderProduct != null)
                    {
                        existingOrderProduct.Quantity += 1; // Örneğin, miktarı güncelleme
                    }
                    else
                    {
                        _orderProductRepository.Add(orderProduct); // Yeni ürün ekleme
                    }
                }

                // Tüm değişiklikleri kaydediyoruz
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage
                {
                    IsSucceed = true,
                    Message = "Order başarıyla güncellendi."
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = $"Order güncellenirken bir hata oluştu: {ex.Message}"
                };
            }
        }


    }
}
