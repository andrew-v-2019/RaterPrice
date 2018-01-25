using System;

namespace RaterPrice.ViewModels.MobileApi.Models
{
    public class GoodPriceViewModel
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public decimal Price { get; set; }
        public int PriceId { get; set; }

        public  int GoodId { get; set; }

        public string GoodName { get; set; }

        public DateTime PriceDate { get; set; }
    }
}