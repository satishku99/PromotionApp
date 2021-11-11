using SKUPromotionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUPromotionApp.Business
{
   public class ProductService
    {
        private readonly List<Product> products;
        private readonly List<ProductSKU> productSkuCollection;

        public ProductService(List<ProductSKU> productSkuCollection)
        {
            this.products = new List<Product>();
            this.productSkuCollection = productSkuCollection;
        }

        public async void AddProduct(Product product)
        {
            if (!productSkuCollection.Any(x => x.Id == product.ProductSKUId)) return;
            var existingProduct = products.Where(x => x.ProductSKUId == product.ProductSKUId).FirstOrDefault();
            if (existingProduct == null) { products.Add(product); return; }
            existingProduct.Quantity += product.Quantity;
        }

        public List<Product> GetProductsItems()
        {
            return  products;
        }
    }
}
