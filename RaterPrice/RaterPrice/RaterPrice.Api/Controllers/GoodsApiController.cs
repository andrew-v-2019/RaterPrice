using RaterPrice.Api.Models.Responses;
using RaterPrice.Api.Services;
using System.Collections.Generic;
using System.Web.Http;
using RaterPrice.Api.Models;

namespace RaterPrice.Api.Controllers
{
    [RoutePrefix("api/goods/")]
    public class GoodsApiController : ApiController
    {

        private IQueryFacade _queryFacade;



        public GoodsApiController(IQueryFacade queryFacade)
        {
            _queryFacade = queryFacade;
        }

        //// GET: GetGoodsByBarcode
        //[HttpGet]
        [Route("search")]
        public List<GoodViewModel> GetByBarcode(string queryBarcode)
        {
            return _queryFacade.GetByBarcode(queryBarcode);
        }

        [Route("search")]
        public List<GoodViewModel> GetByName(string queryName, int max, int skip)
        {
            return _queryFacade.GetByName(queryName, max, skip);
        }

        [Route("getgoods")]
        [HttpGet]
        public List<GoodViewModel> GetGoods([FromUri] GoodsSearchFilterModel filter)
        {
            return _queryFacade.GetGodsByFilteredRequest(filter);
            //var fakeRes = new List<GoodViewModel>();
            //var fakeGood1 = (new GoodViewModel()
            //{
            //    Barcode = "86786786",
            //    Id = 1,
            //    MaxPrice = new GoodPriceViewModel() { Price = 1, ShopId = 45, ShopName = "TestShop" },
            //    MinPrice = new GoodPriceViewModel() { Price = 9.9M, ShopId = 5, ShopName = "TestShop2" },
            //    Name = "TestGood",
            //    ShortName = "Test Good",
            //    IsDraft = true,
            //    Vendor = "Test vendor",
            //    Images = new List<GoodImageViewModel>()

            //});
            //var fakeGood2 = (new GoodViewModel()
            //{
            //    Barcode = "76756756",
            //    Id = 2,
            //    MaxPrice = new GoodPriceViewModel() { Price = 1, ShopId = 45, ShopName = "TestShop" },
            //    MinPrice = new GoodPriceViewModel() { Price = 9.9M, ShopId = 5, ShopName = "TestShop2" },
            //    Name = "TestGood2",
            //    ShortName = "Test Good 2",
            //    IsDraft = false,
            //    Vendor = "Test vendor 2",
            //    Images = new List<GoodImageViewModel>()
            //});

            //fakeGood1.Images.Add(new GoodImageViewModel() { IsMain = true, Name = "TestImage 1", Url = "www" });
            //fakeGood1.Images.Add(new GoodImageViewModel() { IsMain = true, Name = "TestImage 1", Url = "www" });
            //fakeGood2.Images.Add(new GoodImageViewModel() { IsMain = true, Name = "TestImage 2", Url = "www" });
            //fakeGood2.Images.Add(new GoodImageViewModel() { IsMain = true, Name = "TestImage 2", Url = "www" });

            //fakeRes.Add(fakeGood1);
            //fakeRes.Add(fakeGood2);

            //return fakeRes;
        }

        ///api/goods/best?max={x}&skip={y}&cityId={id}
        [Route("best")]
        [HttpGet]
        public List<GoodViewModel> GetBestGoods(int max, int skip, int cityId)
        {
            return _queryFacade.GetBestGoods(max, skip, cityId);
        }

        ///api/goods/{id }
        [Route("{id}")]
        [HttpGet]
        public GoodViewModel GetGoodById(int id)
        {
            return _queryFacade.GetGoodById(id);
        }

        ///api/goods/{id}/prices? cityId = { id }
        [Route("{id}/prices")]
        [HttpGet]
        public List<GoodPriceViewModel> GetPricesInTheCity(int id, int cityId)
        {
            return _queryFacade.GetPrices(id, cityId);
        }

       

    }
}