using System.Collections.Generic;

namespace RaterPrice.ViewModels.MobileApi.Models
{
    public class GoodViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Vendor { get; set; }
        public string Barcode { get; set; }
        public bool IsDraft { get; set; }
        public List<GoodImageViewModel> Images { get; set; }
        public GoodPriceViewModel MinPrice { get; set; }
        public GoodPriceViewModel MaxPrice { get; set; }
    }
}