using System.Collections.Generic;
using RaterPrice.ViewModels.MobileApi.Models;

namespace RaterPrice.Api.Areas.MobileApi.Services
{
    public interface IQueryFacade
    {
        GoodViewModel GetGoodById(int id);
        List<GoodPriceViewModel> GetPrices(int goodId, int cityId);
        List<CityViewModel> GetCities();

        List<ShopViewModel> GetShops(int cityId, string querySearch);

        List<GoodViewModel> GetGoodsByFilter(SearchModel filter);
    }
}