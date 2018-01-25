
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using ImageType = RaterPrice.Api.Areas.SiteApi.Models.Enums.ImageType;
using RaterPrice.Persistence.Domain;
using RaterPrice.ViewModels.MobileApi.Models;

namespace RaterPrice.Api.Areas.MobileApi.Services
{
    public class QueryFacade : IQueryFacade
    {
        private readonly RaterPriceContext _raterPriceContext;

        public QueryFacade(RaterPriceContext raterPriceContext)
        {
            _raterPriceContext = raterPriceContext;
        }

        public List<GoodViewModel> GetGoodsByFilter(SearchModel filter)
        {
           
                var query = GoodViewModelBaseQuery(_raterPriceContext);
                if (!string.IsNullOrEmpty(filter.QueryBarcode))
                {
                    query = query.Where(g => g.Barcode.IndexOf(filter.QueryBarcode) != -1)
                        .Select(g => g);
                }
                FilterGoodsByName(filter.QueryName, ref query);

                if (filter.CityId.HasValue)
                {
                    FilterByCityId(_raterPriceContext, filter.CityId.Value, ref query);
                }
                query = query.OrderBy(g => g.MinPrice.Price).AsQueryable();

                if (filter.Skip.HasValue)
                {
                    query = query.Skip(() => filter.Skip.Value);
                }
                if (filter.Max.HasValue)
                {
                    query = query.Take(() => filter.Max.Value);
                }
                var l = query.AsNoTracking();
                return l.ToList();
            
        }

        private static void FilterGoodsByName(string queryName, ref IQueryable<GoodViewModel> query)
        {
            if (string.IsNullOrEmpty(queryName)) return;
            var filterByName = query.Where(g => g.Name.IndexOf(queryName) > -1)
                .Select(g => g);
            if (!filterByName.Any())
            {
                query = query.Where(g => g.ShortName.IndexOf(queryName) > -1)
                    .Select(g => g);
            }
            else
            {
                query = filterByName;
            }
        }

        public GoodViewModel GetGoodById(int id)
        {
           
                var q = GoodViewModelBaseQuery(_raterPriceContext).Where(g => g.Id == id).Select(g => g).FirstOrDefault();
                return q;
            
        }

        public List<GoodPriceViewModel> GetPrices(int goodId, int cityId)
        {
            
                var q = (from g in _raterPriceContext.Goods
                         join p in _raterPriceContext.Prices on g.Id equals p.GoodId
                         join s in _raterPriceContext.Shops on p.ShopId equals s.Id
                         where g.Id == goodId && s.CityId == cityId
                         select new GoodPriceViewModel
                         {
                             Price = p.PriceValue,
                             ShopId = s.Id,
                             ShopName = s.Name,
                             PriceId = p.Id,
                             GoodId = g.Id,
                             GoodName = g.Name,
                             PriceDate = p.DateUpdated
                         }).OrderBy(p => p.Price).AsNoTracking().ToList();
                return q;
            
        }

        public List<CityViewModel> GetCities()
        {
           
                var list =
                    _raterPriceContext.Cities.Select(c => new CityViewModel { Id = c.Id, Name = c.Name }).AsNoTracking().ToList();
                return list;
            
        }


        public List<ShopViewModel> GetShops(int cityId, string querySearch)
        {
            
                var byCityId = _raterPriceContext.Shops.Where(s => s.CityId == cityId);
                var byPhrase = byCityId.Where(s => s.Name.IndexOf(querySearch) > -1);
                if (!byPhrase.Any())
                {
                    byPhrase = byCityId.Where(s => s.Address.IndexOf(querySearch) > -1);
                }
                var domainlist = byPhrase.Select(s => s).AsNoTracking().ToList();
                var list = domainlist.Select(SelectShopViewModel).ToList();
                return list;
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
                                    select new GoodPriceViewModel { Price = p.PriceValue, ShopId = s.Id, ShopName = s.Name, PriceDate = p.DateUpdated }).OrderByDescending(
                                p => p.Price).FirstOrDefault(),
                        MinPrice = (from p in context.Prices
                                    join s in context.Shops on p.ShopId equals s.Id
                                    select new GoodPriceViewModel { Price = p.PriceValue, ShopId = s.Id, ShopName = s.Name, PriceDate = p.DateUpdated })
                            .OrderBy(p => p.Price).FirstOrDefault(),
                        Images = (from i in context.Images
                                  where i.ObjectId == g.Id && i.ImageTypeId == (int)ImageType.GoodImage
                                  select new GoodImageViewModel { Name = i.FileName, IsMain = i.Main, Url = i.FileUrl }).ToList()
                    };
            return q.OrderBy(g => g.Name);
        }


        private static void FilterByCityId(RaterPriceContext context, int cityId, ref IQueryable<GoodViewModel> query)
        {
            var goodIdsInTheCity = from p in context.Prices
                                   join s in context.Shops on p.ShopId equals s.Id
                                   where s.CityId == cityId
                                   select p.GoodId;
            query = query.Where(g => goodIdsInTheCity.Contains(g.Id));
        }


        private static ShopViewModel SelectShopViewModel(Shop s)
        {
            var model = new ShopViewModel
            {
                Address = s.Address,
                Name = s.Name,
                CityId = s.CityId,
                Id = s.Id,
                IsDraft = s.IsDraft,
                Latitude = s.Latitude,
                Longitude = s.Longitude
            };
            return model;
        }

       
    }
}