using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUPromotionApp.Models
{
    public class ProductSKU
    {
        public ProductSKU(char id, decimal price)
        {
            Id = id;
            Price = price;
        }
        public char Id { get; private set; }
        public decimal Price { get; private set; }
    }
}
