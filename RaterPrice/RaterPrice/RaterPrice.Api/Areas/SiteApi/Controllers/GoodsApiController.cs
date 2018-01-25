using System.Collections.Generic;
using System.Web.Http;
using RaterPrice.Api.Areas.SiteApi.Models;
using RaterPrice.Api.Areas.SiteApi.Services;

namespace RaterPrice.Api.Areas.SiteApi.Controllers
{
    //[RoutePrefix("goods")]
    public class GoodsApiController : ApiController
    {
        private readonly IQueryFacade _queryFacade;


        public GoodsApiController(IQueryFacade queryFacade)
        {
            _queryFacade = queryFacade;
        }


        //http://localhost:53525/siteapi/goodsapi/getgoods?Barcode=&Name=&Page=0&Popular=true&Take=20
        //[Route("getgoods")]
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
    }
}