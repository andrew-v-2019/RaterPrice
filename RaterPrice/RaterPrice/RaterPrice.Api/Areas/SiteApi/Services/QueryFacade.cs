using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RaterPrice.Api.Areas.SiteApi.Models;
using ImageType = RaterPrice.Api.Areas.SiteApi.Models.Enums.ImageType;
using RaterPrice.Persistence.Domain;


namespace RaterPrice.Api.Areas.SiteApi.Services
{
    public class QueryFacade : IQueryFacade
    {
        private readonly RaterPriceContext _raterPriceContext;
        //private readonly IRaterPriceContext _context;

        public QueryFacade(RaterPriceContext raterPriceContext)
        {
            _raterPriceContext = raterPriceContext;
            //_context = context;
        }

        public List<GoodViewModel> GetGodsByFilteredRequest(GoodsSearchFilterModel filter)
        {

            IQueryable<GoodViewModel> query;
            if (!string.IsNullOrEmpty(filter.Barcode))
            {
                query = FilterByBarcode(filter.Barcode, _raterPriceContext);
            }
            else
            {
                query = !string.IsNullOrEmpty(filter.Name) ? FilterByName(filter.Name, _raterPriceContext) : MostPopular(_raterPriceContext);
            }

            var list = query.Skip(filter.Skip).Take(filter.Take).AsNoTracking().ToList();
            return list;

        }

        private static IQueryable<GoodViewModel> FilterByBarcode(string barcode, RaterPriceContext context)
        {
            var l = (GoodViewModelBaseQuery(context)
                .Where(g => g.Barcode.Equals(barcode))
                .Select(g => g));
            return l;
        }

        private static IQueryable<GoodViewModel> MostPopular(RaterPriceContext context)
        {
            var l = (GoodViewModelBaseQuery(context)
                .Select(g => g)).OrderBy(g => g.MinPrice.Price);
            return l;
        }

        private static IQueryable<GoodViewModel> FilterByName(string name, RaterPriceContext context)
        {
            var l = (GoodViewModelBaseQuery(context)
                .Where(g => g.Name.IndexOf(name) > -1).Select(g => g));

            return l;
        }


        private static IQueryable<GoodViewModel> GoodViewModelBaseQuery(RaterPriceContext context)
        {
            var q = from g in context.Goods
                    select new GoodViewModel
                    {
                        Id = g.Id,
                        Name = g.Name,
                        ShortName = g.ShortName,
                        Barcode = g.Barcode,
                        MaxPrice = (from p in context.Prices
                                    join s in context.Shops on p.ShopId equals s.Id
                                    select new GoodPriceViewModel { Price = p.PriceValue, ShopId = s.Id, ShopName = s.Name }).OrderBy(
                                p => p.Price).FirstOrDefault(),
                        MinPrice = (from p in context.Prices
                                    join s in context.Shops on p.ShopId equals s.Id
                                    select new GoodPriceViewModel { Price = p.PriceValue, ShopId = s.Id, ShopName = s.Name })
                            .OrderByDescending(p => p.Price).FirstOrDefault(),
                        Images = (from i in context.Images
                                  where i.ObjectId == g.Id && i.ImageTypeId == (int)ImageType.GoodImage
                                  select new GoodImageViewModel { Name = i.FileName, IsMain = i.Main, Url = i.FileUrl }).ToList()
                    };
            return q.OrderBy(g => g.Name);
        }
    }
}