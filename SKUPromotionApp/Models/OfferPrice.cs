using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUPromotionApp.Models
{
    public class OfferPrice
    {
        public decimal Price { get; set; }
        public List<Product> Products { get; set; }
    }
}
