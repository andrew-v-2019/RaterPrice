using System.Collections.Generic;
using System.Web.Http;
using RaterPrice.Api.Areas.MobileApi.Services;
using RaterPrice.ViewModels.MobileApi.Models;

namespace RaterPrice.Api.Areas.MobileApi.Controllers
{
    public class CitiesApiController : BaseApiController
    {
        private readonly IQueryFacade _queryFacade;

        public CitiesApiController(IQueryFacade queryFacade)
        {
            _queryFacade = queryFacade;
        }


        //http://localhost:53525/api/cities
        [HttpGet]
        public List<CityViewModel> GetCities()
        {
            return _queryFacade.GetCities();
        }
    }
}