using ECommerceCom.Business.Operations.Order.Dtos;
using ECommerceCom.Business.Types;
using ECommerceCom.Data.Entities;
using ECommerceCom.Data.Repositories;
using ECommerceCom.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Feautere.Dtos
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ProductEntity> _repository;
        public ProductManager(IUnitOfWork unitOfWork, IRepository<ProductEntity> repository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceMessage> AddProduct(AddProductDto product)
        {
            var hasProduct = _repository.GetAll(x => x.ProductName.ToLower() == product.ProductName.ToLower() && x.Price == product.Price).Any();
            if (hasProduct) 
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Boyle bir urun bulunuyor"


                };
            }
            var productEntity = new ProductEntity
            {
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity,

            };
            _repository.Add(productEntity);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Product kaydi sirasinda bir hata olustu: {ex.Message}", ex);
            }
            return new ServiceMessage 
            {
                IsSucceed = true
            };
        }
    }
}
