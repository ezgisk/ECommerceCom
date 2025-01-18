using ECommerceCom.Business.Operations.Order.Dtos;
using ECommerceCom.Business.Operations.Product.Dtos;
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
                    Message = "A product with the same name and price already exists."
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
                throw new Exception($"An error occurred while adding the product: {ex.Message}", ex);
            }
            return new ServiceMessage
            {
                IsSucceed = true
            };
        }

        public async Task<ServiceMessage> AdjustProductStockQuantity(int id, int changeTo)
        {
            var product = _repository.GetById(id);
            if (product == null)
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "No product found with the given ID."
                };
            product.StockQuantity = changeTo;
            _repository.Update(product);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adjusting the product: {ex.Message}", ex);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Product successfully updated."
            };
        }

        public async Task<ServiceMessage> DeleteProduct(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "No product found with the given ID."
                };
            }
            _repository.Delete(id);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the product: {ex.Message}", ex);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Product successfully deleted."
            };
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var products = _repository.GetAll();

            if (products == null || !products.Any())
            {
                return new List<ProductDto>(); // Return an empty list if no products are found
            }

            // Convert products to ProductDto
            var productDtos = products.Select(product => new ProductDto
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            }).ToList();

            return productDtos;
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var product = _repository.GetById(id);

            // If product is not found, return null or an appropriate error message
            if (product == null)
            {
                return null; 
            }

            // If product is found, convert it to ProductDto
            var productDto = new ProductDto
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };

            return productDto; 
        }

        public async Task<ServiceMessage> UpdateProduct(UpdateProductDto product)
        {
            var productEntity = _repository.GetById(product.ProductId);

            if (productEntity == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Product not found."
                };
            }

            // Start a transaction at the beginning of the process
            await _unitOfWork.BeginTransaction();

            try
            {
                // Get the new values from the DTO
                if (!string.IsNullOrEmpty(product.ProductName))
                {
                    productEntity.ProductName = product.ProductName; // Update product name
                }

                if (product.Price > 0)
                {
                    productEntity.Price = product.Price; // Update price
                }

                if (product.StockQuantity >= 0)
                {
                    productEntity.StockQuantity = product.StockQuantity; // Update stock quantity
                }

                // Update the product
                _repository.Update(productEntity);

                
                await _unitOfWork.SaveChangesAsync();

                // Commit the transaction if everything is successful
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage
                {
                    IsSucceed = true,
                    Message = "Product successfully updated."
                };
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = $"An error occurred while updating the product: {ex.Message}"
                };
            }
        }
    }
}
