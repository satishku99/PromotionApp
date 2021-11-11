using SKUPromotionApp.Business;
using SKUPromotionApp.BusinessServices;
using SKUPromotionApp.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace SKUPromotionsAppTest
{
    public class CalculationEngineUnitTest
    {
        [Fact]
        public void SenarioA_No_Offer_ShouldReturnTotalPriceOfTheProductItems()
        {
            // Arrange
            var productSkuCollection = new List<ProductSKU>
            {
                new ProductSKU('A', 50), new ProductSKU('B', 30), new ProductSKU('C', 20),new ProductSKU('D', 15)
            };
            var productOffer = new ProductOffers(productSkuCollection);

            var products = new ProductService(productSkuCollection);
            products.AddProduct(new Product { Quantity = 1, ProductSKUId = 'A' });
            products.AddProduct(new Product { Quantity = 1, ProductSKUId = 'B' });
            products.AddProduct(new Product { Quantity = 1, ProductSKUId = 'c' });
            var cashier = new ClaculationEngineService(products, productOffer, productSkuCollection);

            // Act
            var result = cashier.Calculate();

            // Assert
            Assert.Equal(200, result);
        }

     
        [Fact]
        public void SenarioB_ShouldReturnTotalCostOfTheCartItemsWithDiscount()
        {
            // Arrange
            var productSkuCollection = new List<ProductSKU>
            {
                new ProductSKU('A', 50), new ProductSKU('B', 30), new ProductSKU('C', 20),new ProductSKU('D', 15)
            };
            var promotionalOffer = new ProductOffers(productSkuCollection);
            promotionalOffer.AddOffer(new OfferPrice { Price = 130, Products = new List<Product> { new Product { Quantity = 3, ProductSKUId = 'A' } } });
            promotionalOffer.AddOffer(new OfferPrice { Price = 30, Products = new List<Product> { new Product { Quantity = 1, ProductSKUId = 'C' }, new Product { Quantity = 1, ProductSKUId = 'D' } } });

            var products = new ProductService(productSkuCollection);
            products.AddProduct(new Product { Quantity = 5, ProductSKUId = 'A' });
            products.AddProduct(new Product { Quantity = 5, ProductSKUId = 'B' });
            products.AddProduct(new Product { Quantity = 1, ProductSKUId = 'C' });

            var cashier = new ClaculationEngineService(products, promotionalOffer, productSkuCollection);

            // Act
            var result = cashier.Calculate();

            // Assert
            Assert.Equal(370, result);
        }
        [Fact]
        public void SenarioC_ShouldReturnTotalCostOfTheCartItemsWithDiscount()
        {
            // Arrange
            var productSkuCollection = new List<ProductSKU>
            {
                new ProductSKU('A', 50), new ProductSKU('B', 30), new ProductSKU('C', 20),new ProductSKU('D', 15)
            };
            var promotionalOffer = new ProductOffers(productSkuCollection);
            promotionalOffer.AddOffer(new OfferPrice { Price = 130, Products = new List<Product> { new Product { Quantity = 3, ProductSKUId = 'A' } } });
            promotionalOffer.AddOffer(new OfferPrice { Price = 30, Products = new List<Product> { new Product { Quantity = 1, ProductSKUId = 'C' }, new Product { Quantity = 1, ProductSKUId = 'D' } } });

            var products = new ProductService(productSkuCollection);
            products.AddProduct(new Product { Quantity = 3, ProductSKUId = 'A' });
            products.AddProduct(new Product { Quantity = 5, ProductSKUId = 'B' });
            products.AddProduct(new Product { Quantity = 1, ProductSKUId = 'C' });
            products.AddProduct(new Product { Quantity = 1, ProductSKUId = 'D' });
            var cashier = new ClaculationEngineService(products, promotionalOffer, productSkuCollection);

            // Act
            var result = cashier.Calculate();

            // Assert
            Assert.Equal(380, result);
        }
    }
}
