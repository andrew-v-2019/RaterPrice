
using RaterPrice.Infrastructure;
using RaterPrice.Persistence.Domain;
using RaterPrice.Services.DTO;
using RaterPrice.ViewModels.MobileApi.Models;
using RaterPrice.ViewModels.MobileApiModels.Registration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RaterPrice.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly RaterPriceContext _raterPriceContext;

        private readonly ISmsService _smsService;
        private IUserService _userService;
        

        public RegistrationService(RaterPriceContext raterPriceContext, ISmsService smsService, IUserService userService )
        {
            
            _raterPriceContext = raterPriceContext;
            _smsService = smsService;
            _userService = userService;
        }

        private ConfirmationCode FindActiveCodeForUser(string code, int userId)
        {
            
                var domain = _raterPriceContext.ConfirmationCodes
                    .Where(c => c.Code.Equals(code) && c.UserId == userId)
                    .Select(c => c).FirstOrDefault();
                return domain;
            
        }

        private bool CheckCodeDate(string code, int userId)
        {
            var domain = FindActiveCodeForUser(code, userId);
            var diff = domain.CreationDate - DateTime.UtcNow;
            if (diff.Hours > 24)
                return false;
            return true;
        }

        public SendSmsResultViewModel RegisterUser(RegisterUserViewModel model)
        {
            model.Phone = Extensions.FormatPhoneNumber(model.Phone);
            if (!_userService.CheckPhone(model.Phone))
            {
                return new SendSmsResultViewModel()
                {
                    Errors = new List<string>(new[] { "Такой телефон уже есть в базе" }),
                };
            }
            User user;
            user = _userService.CreateSimpleUser(model);
            var sendResult = SendConfirmationCode(user);
            return sendResult;
        }

        public ConfirmationResultViewModel ConfirmUser(string code, int userId)
        {
            var model = new ConfirmationResultViewModel();
            model.Token = null;
            model.Confirmed = false;
            var exist = FindActiveCodeForUser(code, userId);
            if (exist == null)
            {
                model.Status = ConfirmationResults.CodeNotFound.ToString();
                return model;
            }
            var young = CheckCodeDate(code, userId);
            if (!young)
            {
                model.Status = ConfirmationResults.CodeExpired.ToString();
                return model;
            }
            ConfirmCode(exist.Id);
            model.Token = Guid.NewGuid();
            
                var user = _raterPriceContext.Users.Where(u => u.Id == userId).Select(u => u).First();
                user.Token = (Guid)model.Token;
                user.ConfirmationDate = DateTime.UtcNow;
                _raterPriceContext.SaveChanges();
            
            model.Confirmed = true;
            model.Status = ConfirmationResults.Success.ToString();

            return model;
        }

        private void ConfirmCode(int codeId)
        {
            
                var currentCode = _raterPriceContext.ConfirmationCodes.Where(c => c.Id == codeId).Select(c => c).First();
            _raterPriceContext.ConfirmationCodes.Remove(currentCode);
            _raterPriceContext.SaveChanges();
            
        }

        private string GenerateCode()
        {
            var r = new Random();
            
                string code;
                do
                {
                    code = r.Next(10000, 99999).ToString();
                } while (_raterPriceContext.ConfirmationCodes.Any(c => c.Code.Equals(code)));
                return code;
            
        }

        private SendSmsResultViewModel SendConfirmationCode(User user)
        {
            var model = new SendSmsResultViewModel()
            {
                Errors = new List<string>()
            };
            var code = GenerateCode();
            var message = "Ваш код подтверждения " + code;
            var sendSmsResult = _smsService.CreateAndSendSms(message, user);
            model.SmsMessageSendId = sendSmsResult.SendSmsId;
            if (sendSmsResult.RequestResult.ErrorCode != null)
            {
                model.Errors.Add(sendSmsResult.RequestResult.ErrorText);
                return model;
            }

            RegisterConfirmationCode(user.Id, code);

            var checkStatusResult = _smsService.CheckAndUpdateSmsStatus((int)sendSmsResult.SendSmsId, user.PhoneNumber);
            if (checkStatusResult.RequestResult.ErrorCode != null)
            {
                model.Errors.Add(checkStatusResult.RequestResult.ErrorText);
            }
            model.SmsStatus = checkStatusResult.Status;
            model.StatusInfo = checkStatusResult.StatusText;
            model.UserId = user.Id;
            model.CodeTmp = code; // TODO: УБРАТЬ ЭТО !!!
            return model;
        }

        private ConfirmationCode RegisterConfirmationCode(int userId, string code)
        {
            CancelAllConfirmationCodes(userId);
            var codeDomain = new ConfirmationCode();
            
                codeDomain.Code = code;
                codeDomain.CreationDate = DateTime.UtcNow;
                codeDomain.UserId = userId;
            _raterPriceContext.ConfirmationCodes.Add(codeDomain);
            _raterPriceContext.SaveChanges();
                return codeDomain;
            
        }

        private void CancelAllConfirmationCodes(int userId)
        {
            
                var codes = _raterPriceContext.ConfirmationCodes.Where(c => c.UserId == userId).Select(c => c).AsNoTracking().ToList();
            _raterPriceContext.ConfirmationCodes.RemoveRange(codes);
            _raterPriceContext.SaveChanges();
            
        }


    }
}
