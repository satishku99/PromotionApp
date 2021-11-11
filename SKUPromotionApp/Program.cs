using SKUPromotionApp.Business;
using SKUPromotionApp.BusinessServices;
using SKUPromotionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUPromotionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Add all possible SKU's
            var productSkuList = new List<ProductSKU>
            {
                new ProductSKU('A', 50),
                new ProductSKU('B', 30),
                new ProductSKU('C', 20),
                new ProductSKU('D', 15)
            };

            // Create promotional offers
            var promotionEngine = GetProductOffer(productSkuList);

            // Scenario - 1
            var cart1 = new ProductService(productSkuList);
            cart1.AddProduct(new Product { Quantity = 1, ProductSKUId = 'A' });
            cart1.AddProduct(new Product { Quantity = 1, ProductSKUId = 'B' });
            cart1.AddProduct(new Product { Quantity = 1, ProductSKUId = 'C' });
            var cashier = new ClaculationEngineService(cart1, promotionEngine, productSkuList);
            Console.WriteLine("Order 1: " + cashier.Calculate());

            // Scenario - 2
            var cart2 = new ProductService(productSkuList);
            cart2.AddProduct(new Product { Quantity = 5, ProductSKUId = 'A' });
            cart2.AddProduct(new Product { Quantity = 5, ProductSKUId = 'B' });
            cart2.AddProduct(new Product { Quantity = 1, ProductSKUId = 'C' });
            cashier = new ClaculationEngineService(cart2, promotionEngine, productSkuList);
            Console.WriteLine("Order 2: " + cashier.Calculate());

            // Scenario - 3
            var cart3 = new ProductService(productSkuList);
            cart3.AddProduct(new Product { Quantity = 3, ProductSKUId = 'A' });
            cart3.AddProduct(new Product { Quantity = 5, ProductSKUId = 'B' });
            cart3.AddProduct(new Product { Quantity = 1, ProductSKUId = 'C' });
            cart3.AddProduct(new Product { Quantity = 1, ProductSKUId = 'D' });
            cashier = new ClaculationEngineService(cart3, promotionEngine, productSkuList);
            Console.WriteLine("Order 3: " + cashier.Calculate());

        }

        private static ProductOffers GetProductOffer(List<ProductSKU> skuList)
        {
            var promotionalOffer = new ProductOffers(skuList);
            promotionalOffer.AddOffer(new OfferPrice
            {
                Price = 130,
                Products = new List<Product>
                {
                    new Product { Quantity = 3, ProductSKUId = 'A' }
                }
            });

            promotionalOffer.AddOffer(new OfferPrice
            {
                Price = 45,
                Products =
                new List<Product>
                {
                    new Product { Quantity = 2, ProductSKUId = 'B' }
                }
            });

            promotionalOffer.AddOffer(new OfferPrice
            {
                Price = 30,
                Products = new List<Product>
                {
                    new Product { Quantity = 1, ProductSKUId = 'C' },
                    new Product { Quantity = 1, ProductSKUId = 'D' }
                }
            });

            return promotionalOffer;
        }
    }
}
