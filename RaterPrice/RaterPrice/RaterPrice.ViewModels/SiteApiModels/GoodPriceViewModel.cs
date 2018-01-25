namespace RaterPrice.Api.Areas.SiteApi.Models
{
    public class GoodPriceViewModel
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public decimal Price { get; set; }
        public int PriceId { get; set; }
    }
}