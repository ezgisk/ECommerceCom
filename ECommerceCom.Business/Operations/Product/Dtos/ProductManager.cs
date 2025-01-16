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

        public async Task<ServiceMessage> AdjustProductStockQuantity(int id, int changeTo)
        {
            var product = _repository.GetById(id);
            if (product == null)
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu id ile eslesen product bulanamadi."
                };
            product.StockQuantity = changeTo;
            _repository.Update(product);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Product degistirilirken bir hata oluştu: {ex.Message}", ex);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Product başarıyla oluşturuldu."
            };
        }

        public async Task<ServiceMessage> DeleteProduct(int id)
        {
            var order = _repository.GetById(id);
            if (order == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu id ile eslesen product bulanamadi."
                };
            }
            _repository.Delete(id);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Product silinirken bir hata oluştu: {ex.Message}", ex);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Product başarıyla silindi."
            };
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var products = _repository.GetAll(); // veya _context.Products.ToListAsync() kullanabilirsiniz.

            if (products == null || !products.Any())
            {
                return new List<ProductDto>(); // Eğer ürün yoksa boş liste döndür
            }

            // Ürünleri ProductDto'ya dönüştürüyoruz
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
            var product =  _repository.GetById(id);

            // Ürün bulunamazsa, null dönebiliriz veya uygun bir hata mesajı döndürebiliriz
            if (product == null)
            {
                return null; // veya hata mesajı döndürebilirsiniz
            }

            // Ürün bulunduysa, ProductDto'ya dönüştürüyoruz
            var productDto = new ProductDto
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };

            return productDto; // DTO'yu döndürüyoruz
        }

        public async Task<ServiceMessage> UpdateProduct(UpdateProductDto product)
        {
            var productEntity = _repository.GetById(product.ProductId);

            if (productEntity == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Product bulunamadı."
                };
            }

            // İşlem başlangıcında bir transaction başlatıyoruz
            await _unitOfWork.BeginTransaction();

            try
            {
                // Gelen DTO'dan yeni değerleri alıyoruz
                if (!string.IsNullOrEmpty(product.ProductName))
                {
                    productEntity.ProductName = product.ProductName; // Ürün adını güncelle
                }

                if (product.Price > 0)
                {
                    productEntity.Price = product.Price; // Fiyatı güncelle
                }

                if (product.StockQuantity >= 0)
                {
                    productEntity.StockQuantity = product.StockQuantity; // Stok miktarını güncelle
                }

                // Ürünü güncelliyoruz
                _repository.Update(productEntity);

                // Değişiklikleri kaydediyoruz
                await _unitOfWork.SaveChangesAsync();

                // Transaction'ı başarılı şekilde commit ediyoruz
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage
                {
                    IsSucceed = true,
                    Message = "Product başarıyla güncellendi."
                };
            }
            catch (Exception ex)
            {
                // Hata durumunda transaction'ı geri alıyoruz
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = $"Product güncellenirken bir hata oluştu: {ex.Message}"
                };
            }
        }
    }
 }

