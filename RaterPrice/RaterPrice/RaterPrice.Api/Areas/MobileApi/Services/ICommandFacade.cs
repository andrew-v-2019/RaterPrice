using RaterPrice.Persistence.Domain;
using RaterPrice.ViewModels.MobileApi.Models;

namespace RaterPrice.Api.Areas.MobileApi.Services
{
    public interface ICommandFacade
    {
        Price AddGoodPrice(AddPriceModel price);
    }
}