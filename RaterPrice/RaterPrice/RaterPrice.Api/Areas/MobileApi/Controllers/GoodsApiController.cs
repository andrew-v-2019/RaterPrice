using System.Collections.Generic;
using System.Web.Http;
using RaterPrice.Api.Areas.MobileApi.Services;
using RaterPrice.ViewModels.MobileApi.Models;

namespace RaterPrice.Api.Areas.MobileApi.Controllers
{

    public class GoodsMobileApiController : BaseApiController
    {
        private readonly IQueryFacade _queryFacade;
        private readonly ICommandFacade _commandFacade;

        public GoodsMobileApiController(IQueryFacade queryFacade, ICommandFacade commandFacade)
        {
            _queryFacade = queryFacade;
            _commandFacade = commandFacade;
        }

        //http://localhost:53525/api/goods/best?max=1&skip=23&cityId=1
        //http://localhost:53525/api/goods/search?queryBarcode=12&queryName=&max=1&skip=8
        [HttpGet]
        public List<GoodViewModel> Search([FromUri] SearchModel filter)
        {
            return _queryFacade.GetGoodsByFilter(filter);
        }

        //http://localhost:53525/api/goods/222
        [HttpGet]
        public GoodViewModel GetGoodById(int id)
        {
            return _queryFacade.GetGoodById(id);
        }

        //http://localhost:53525/api/goods/222/prices?cityId=22
        [HttpGet]
        public List<GoodPriceViewModel> GetPricesInTheCity(int id, int cityId)
        {
            return _queryFacade.GetPrices(id, cityId);
        }

        ///api/goods/prices
        [HttpPost]

        public IHttpActionResult AddGoodPrice([FromUri] int goodid, [FromBody] AddPriceModel model)
        {
            model.GoodId = goodid;
            var r = _commandFacade.AddGoodPrice(model);
            return SuccessResult(r);
        }
    }
}