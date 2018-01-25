using RaterPrice.Persistence.Domain;
using RaterPrice.SMS.DTO;

namespace RaterPrice.Services
{
    public interface ISmsService
    {
        SendSmsResult CreateAndSendSms(string message, User user);
        CheckSmsStatusResult CheckAndUpdateSmsStatus(int smsSendId, string phone);
    }
}