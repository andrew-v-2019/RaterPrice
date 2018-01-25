namespace RaterPrice.ViewModels.MobileApi.Models
{
    public class SearchModel
    {
        public string QueryBarcode { get; set; }

        public string QueryName { get; set; }

        public int? Max { get; set; }

        public int? Skip { get; set; }

        public int? CityId { get; set; }
    }
}