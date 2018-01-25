
using RaterPrice.Persistence.Domain;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;


namespace RaterPrice.FillTablesWithTestData
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["raterPriceConnectionString"].ConnectionString;

            using (RaterPriceContext context = RaterPriceContext.Create())
            {
                var goodsIdsWithPrices = context.Prices.Select(p => p.GoodId);
                var goodsWithoutPrice = context.Goods.Where(g => !goodsIdsWithPrices.Contains(g.Id)).Select(g => g).AsNoTracking().ToList();

                if (!goodsWithoutPrice.Any())
                {
                    goodsWithoutPrice = context.Goods.Select(g => g).AsNoTracking().ToList();
                }


                foreach (var good in goodsWithoutPrice)
                {
                    var shopsIdsWithPrices = context.Prices.Select(p => p.ShopId);
                    var shops = context.Shops.Where(s => !shopsIdsWithPrices.Contains(s.Id)).Select(s => s);

                    if (shops.Count()<2)
                    {
                        shops = context.Shops.Select(s => s);
                    }

                    var r1 = new Random();
                    var take = r1.Next(1, shops.Count()-1);
                    var skip = r1.Next(0, take - 1);
                    var workingRange = shops.OrderBy(s=>s.Id).Skip(() => skip).Take(() => take).AsNoTracking().ToList();

                    foreach (var shop in workingRange)
                    {
                        if (context.Prices.Any(p => p.ShopId == shop.Id && p.GoodId == good.Id)) continue;
                        var r = new Random();
                        var price = new Price()
                        {
                            GoodId = good.Id,
                            ShopId = shop.Id,
                            DateUpdated = DateTime.UtcNow,
                            PriceValue = Convert.ToDecimal(r.Next(1, 1000) + r.NextDouble()),
                            UserId = r.Next()
                        };
                        context.Prices.Add(price);
                        context.SaveChanges();
                        Console.WriteLine(price.PriceValue + "->" + shop.Name + "->" + good.Name);
                    }
                }
            }
        }
    }
}
