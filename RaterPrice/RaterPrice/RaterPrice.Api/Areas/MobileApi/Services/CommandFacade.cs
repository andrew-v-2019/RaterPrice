using System;
using System.Linq;
using RaterPrice.Persistence.Domain;
using RaterPrice.ViewModels.MobileApi.Models;

namespace RaterPrice.Api.Areas.MobileApi.Services
{
    public class CommandFacade: ICommandFacade
    {
        private readonly RaterPriceContext _raterPriceContext;

        public CommandFacade(RaterPriceContext raterPriceContext)
        {
            _raterPriceContext = raterPriceContext;
        }

        public Price AddGoodPrice(AddPriceModel price)
        {
            
                var domain = new Price
                {
                    DateUpdated = DateTime.UtcNow,
                    GoodId = price.GoodId,
                    PriceValue = price.PriceValue,
                    ShopId = price.ShopId
                };
                //domain.UserId=

                var existing =
                    _raterPriceContext.Prices.Where(p => p.ShopId == price.ShopId && p.GoodId == price.GoodId)
                        .Select(p => p)
                        .FirstOrDefault();
                if (existing == null)
                {
                _raterPriceContext.Prices.Add(domain);
                }
                else
                {
                    existing.PriceValue = price.PriceValue;
                    existing.DateUpdated = DateTime.UtcNow;
                }
            _raterPriceContext.SaveChanges();
                return existing;
            }
        }
    }