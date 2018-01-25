namespace RaterPrice.ViewModels.MobileApi.Models
{
    public class ShopViewModel
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsDraft { get; set; }
    }
}