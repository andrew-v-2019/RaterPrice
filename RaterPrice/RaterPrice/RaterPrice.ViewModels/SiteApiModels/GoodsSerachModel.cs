namespace RaterPrice.Api.Areas.SiteApi.Models
{
    public class GoodsSearchFilterModel : PagedSearchRequest
    {
        public bool Popular { get; set; }

        public string Barcode { get; set; }

        public string Name { get; set; }
    }
}