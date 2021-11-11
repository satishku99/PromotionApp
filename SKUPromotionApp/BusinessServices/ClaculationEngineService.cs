using SKUPromotionApp.Business;
using SKUPromotionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUPromotionApp.BusinessServices
{
    public class ClaculationEngineService
    {
        private readonly List<OfferPrice> _offerItems;
        private readonly List<ProductSKU> _productSkuCollection;
        private readonly ProductService _productService;

        public ClaculationEngineService(ProductService productService, ProductOffers offerItems, List<ProductSKU> productSkuCollection)
        {
            _productSkuCollection = productSkuCollection;
            _productService = productService;
            _offerItems = offerItems.GetPromotions();

        }
        public decimal Calculate()
        {
            var productList = _productService.GetProductsItems().ConvertAll(product => new ProductStatus(product, false));
            var total = 0.0m;
            for (int i = 0; i < productList.Count; i++)
            {
                var currentProduct = productList[i];
                if (currentProduct.IsProcessed == true) continue;
                var filteredPromotionalItems = _offerItems.Where(x => x.Products.Any(y => y.ProductSKUId == currentProduct.Product.ProductSKUId && y.Quantity <= currentProduct.Product.Quantity));


                if (!filteredPromotionalItems.Any())   // When there is No offer Full Price calculation
                {
                    total += _productSkuCollection.First(x => x.Id == currentProduct.Product.ProductSKUId).Price * currentProduct.Product.Quantity;
                    currentProduct.IsProcessed = true;
                }
                else
                {
                    foreach (var promotionalItem in filteredPromotionalItems) //When offer is available for single product
                    {

                        if (promotionalItem.Products.Count == 1)
                        {
                            var quantity = currentProduct.Product.Quantity / promotionalItem.Products[0].Quantity;
                            total += quantity * promotionalItem.Price;
                            var remainingProductQuantity = PendingProducts(currentProduct.Product.Quantity, promotionalItem.Products[0].Quantity);
                            if (remainingProductQuantity > 0)
                            {
                                productList.Add(new ProductStatus(new Product { Quantity = remainingProductQuantity, ProductSKUId = currentProduct.Product.ProductSKUId }, false));
                            }
                            currentProduct.IsProcessed = true;
                        }
                        else // Offer for Multiple products
                        {
                            var currentPromotionalItem = promotionalItem.Products.First(x => x.ProductSKUId == currentProduct.Product.ProductSKUId);
                            var otherPromotionalMandatoryItems = promotionalItem.Products.Where(x => x.ProductSKUId != currentProduct.Product.ProductSKUId);
                            var otherUnprocessedProduct = productList.Where(x => x.IsProcessed == false & otherPromotionalMandatoryItems.All(y => x.Product.Quantity >= y.Quantity && x.Product.ProductSKUId == y.ProductSKUId));
                            if (otherUnprocessedProduct.Any())
                            {
                                foreach (var promotionalItemProduct in otherPromotionalMandatoryItems)
                                {
                                    var unprocessedProduct = otherUnprocessedProduct.First(x => x.Product.ProductSKUId == promotionalItemProduct.ProductSKUId);
                                    var remainingOtherProductQuantity = PendingProducts(unprocessedProduct.Product.Quantity, promotionalItemProduct.Quantity);
                                    if (remainingOtherProductQuantity > 0)
                                    {
                                        productList.Add(new ProductStatus(new Product { Quantity = remainingOtherProductQuantity, ProductSKUId = unprocessedProduct.Product.ProductSKUId }, false));
                                    }
                                    unprocessedProduct.IsProcessed = true;
                                }

                                var remainingProductQuantity = PendingProducts(currentProduct.Product.Quantity, currentPromotionalItem.Quantity);
                                if (remainingProductQuantity > 0)
                                {
                                    productList.Add(new ProductStatus(new Product { Quantity = remainingProductQuantity, ProductSKUId = currentProduct.Product.ProductSKUId }, false));
                                }
                                total += promotionalItem.Price;
                                currentProduct.IsProcessed = true;
                            }
                        }
                    }
                }
            }

            foreach (var product in productList.Where(x => x.IsProcessed == false))
            {
                total += _productSkuCollection.First(x => x.Id == product.Product.ProductSKUId).Price * product.Product.Quantity;
                product.IsProcessed = true;
            }

            return total;
        }

        private int PendingProducts(int quantity, int offerQuantity)
        {
            if (offerQuantity == 1) return quantity - offerQuantity;
            return quantity % offerQuantity;
        }
    }
}
