using System.Collections.Generic;
using System.Web.Http;

using RaterPrice.Api.Areas.MobileApi.Services;
using RaterPrice.ViewModels.MobileApi.Models;

namespace RaterPrice.Api.Areas.MobileApi.Controllers
{
    public class ShopsApiController : BaseApiController
    {
        private readonly IQueryFacade _queryFacade;

        public ShopsApiController(IQueryFacade queryFacade)
        {
            _queryFacade = queryFacade;
        }


        //http://localhost:53525/api/shops/search?cityId=1&querySearch=ОАО
        [HttpGet]
        public List<ShopViewModel> GetShops(int cityId, string querySearch)
        {
            return _queryFacade.GetShops(cityId, querySearch);
        }
    }
}