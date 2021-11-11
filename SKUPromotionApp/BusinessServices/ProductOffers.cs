using SKUPromotionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUPromotionApp.BusinessServices
{
   public class ProductOffers
    {

        private readonly List<OfferPrice> offerPrices;
        private readonly List<ProductSKU> skuCollection;
        public ProductOffers(List<ProductSKU> skuCollection)
        {
            this.skuCollection = skuCollection;
            this.offerPrices = new List<OfferPrice>();
        }

        public void AddOffer(OfferPrice offer)
        {
            var productSku_ids = skuCollection.Select(y => y.Id);
            if (!offer.Products.All(x => productSku_ids.Any(y => y == x.ProductSKUId))) return;
            offerPrices.Add(offer);
        }

        public List<OfferPrice> GetPromotions()
        {
            var offerPriceList = new List<(decimal, OfferPrice)>();
            foreach (var offerPrice in offerPrices)
            {
                decimal actualPrice = 0;
                foreach (var product in offerPrice.Products)
                {
                    var sku = skuCollection.First(x => x.Id == product.ProductSKUId);
                    actualPrice += sku.Price * product.Quantity;
                }
                var discountValue = actualPrice - offerPrice.Price;
                var discountPercent = (discountValue * 100) / actualPrice;

                offerPriceList.Add((discountPercent, offerPrice));
            }
            return offerPriceList.OrderByDescending(x => x.Item1).Select(y => y.Item2).ToList();
        }
    }
}
