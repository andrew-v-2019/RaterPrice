using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RaterPrice.Api.Infrastructure;
using RaterPrice.Infrastructure;
using RaterPrice.Persistence.Domain;
using RaterPrice.Services;
using RaterPrice.ViewModels.SiteApiModels.Account;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RaterPrice.Api.Areas.SiteApi.Controllers
{

    public class AccountApiController : ApiController
    {
        private readonly RaterPriceUserManager _userManager;
        private readonly SignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountApiController(SignInManager SignInManager, RaterPriceUserManager userManager, IAuthenticationManager authManager)
        {
            _userManager = userManager;// HttpContext.Current.GetOwinContext().GetUserManager<RaterPriceUserManager>();
            _signInManager = SignInManager;//HttpContext.Current.GetOwinContext().GetUserManager<SignInManager>();
            _authenticationManager = authManager;
        }

        [Route("logout")]
        [HttpPost]   
        [Authorize]
        public void LogOut()
        {
            _authenticationManager.SignOut();
        }


        [HttpPost]
        [AllowAnonymous]    
        public async Task<IHttpActionResult> Login([FromBody]LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true ,shouldLockout: false);
            if (result == SignInStatus.Success)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                UserData viewModel = new UserData { UserName = user.UserName };
                return Json(viewModel);
            }
            return BadRequest("Данные не верны");
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new User() { UserName = model.RegisterEmail, Email = model.RegisterEmail };
            IdentityResult result =  _userManager.Create(user, model.RegisterPassword);

            if (!result.Succeeded)
            {
                string[] array = result.Errors.Cast<string>().ToArray();
                return BadRequest(array[0]);
            }           
            var code= await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl =string.Format("raterprice.ru/account/confirmemail?userId={0}&token={1}",user.Id, code);
            await _userManager.SendEmailAsync(user.Id,
               "Confirm your account",
               "Please confirm your account by clicking this link: <a href=\""
               + callbackUrl + "\">link</a>");
            return Ok();
        }
    }
}
