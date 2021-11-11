using SKUPromotionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUPromotionApp.Business
{
    class ProductStatus
    {
        public ProductStatus(Product product, bool isProcessed)
        {
            Product = product;
            IsProcessed = isProcessed;
        }
        public Product Product { get; private set; }
        public bool IsProcessed { get; set; }
      
    }
}
