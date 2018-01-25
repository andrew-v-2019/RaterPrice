using RaterPrice.SMS.DTO;
using System.Collections.Generic;

namespace RaterPrice.SMS
{
    public interface ISmsGateWay
    {
        SendSmsResult SendSms(List<string> phones, string message, int messageId, string senderName);
        CheckSmsStatusResult GetStatus(int smsSendId, string phone);
    }
}
