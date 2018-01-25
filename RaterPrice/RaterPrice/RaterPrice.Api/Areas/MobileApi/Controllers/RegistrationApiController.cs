using RaterPrice.Services;
using RaterPrice.ViewModels.MobileApi.Models;
using RaterPrice.ViewModels.MobileApiModels.Registration;
using System.Web.Http;

namespace RaterPrice.Api.Areas.MobileApi.Controllers
{
    public class RegistrationApiController : BaseApiController
    {

        private readonly IRegistrationService _registrationService;
        public RegistrationApiController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }


        [HttpPost]
        public IHttpActionResult Registration([FromBody]RegisterUserViewModel registrationModel)
        {
            if (!ModelState.IsValid)
            {
                return ErrorResult(ModelState);
            }
            var result = _registrationService.RegisterUser(registrationModel);
            if (result.Errors!=null)
            {
                return ErrorResult(ModelState);
            }
            return SuccessResult(result);
        }

        [HttpPost]
        public IHttpActionResult ConfirmRegistration([FromBody]ConfirmRegistrationViewModel registrationModel)
        {
            if (!ModelState.IsValid)
            {
                return ErrorResult(ModelState);
            }
            var model = _registrationService.ConfirmUser(registrationModel.ConfirmationCode, registrationModel.UserId);
            if (model.Confirmed)
            {
                return SuccessResult(model);
            }
            return ErrorResult(0, model.Status);
        }
    }
}

