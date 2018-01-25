using System;


namespace RaterPrice.ViewModels.MobileApi.Models
{
    public class AddPriceModel
    {
        public int ShopId { get; set; }

        public Guid? Token { get; set; }

        public decimal PriceValue { get; set; }

        public int GoodId { get; set; }

    }
}