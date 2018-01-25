using System.Collections.Generic;
using RaterPrice.Api.Areas.SiteApi.Models;

namespace RaterPrice.Api.Areas.SiteApi.Services
{
    public interface IQueryFacade
    {
        List<GoodViewModel> GetGodsByFilteredRequest(GoodsSearchFilterModel filter);
    }
}