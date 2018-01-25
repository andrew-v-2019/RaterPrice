
using RaterPrice.Persistence.Domain;
using RaterPrice.SMS;
using RaterPrice.SMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RaterPrice.Services
{
    public class SmsService:ISmsService
    {
        private readonly ISmsGateWay _smsGateWay;
        private readonly RaterPriceContext _raterPriceContext;
        private const int smsSenderId = 1;

        public SmsService(ISmsGateWay smsGateWay, RaterPriceContext raterPriceContext)
        {
            _smsGateWay = smsGateWay;
            _raterPriceContext= raterPriceContext;
        }

        public SendSmsResult CreateAndSendSms(string message, User user)
        {
            SmsSender sender;
            var smsMessage = new SmsMessage()
            {
                Text = message
            };
            var send = new SmsMessageSend();

           
                sender = _raterPriceContext.SmsSenders.Where(s => s.Id == smsSenderId).Select(s => s).First();

            _raterPriceContext.SmsMessages.Add(smsMessage);
            _raterPriceContext.SaveChanges();
                send.ServiceId = _smsGateWay.GetType().ToString();
                send.SmsSenderId = sender.Id;
                send.SmsMessageId = smsMessage.Id;
                send.UserId = user.Id;
                send.CreationDate = null;
                send.StatusChangeDate = null;
                send.Status = null;
            _raterPriceContext.SmsMessageSends.Add(send);
            _raterPriceContext.SaveChanges();
            
            var sendResult = _smsGateWay.SendSms(new List<string>() { user.PhoneNumber }, smsMessage.Text, send.Id, sender.Name);
            
                var updatingSend = _raterPriceContext.SmsMessageSends.Where(s => s.Id == send.Id).Select(s => s).First();
                updatingSend.ResponseToSendSmsRequest = sendResult.RequestResult.TextView;
                if (sendResult.RequestResult.ErrorCode != null)
                {
                    updatingSend.ErrorCode = sendResult.RequestResult.ErrorCode;
                }
                else
                {
                    updatingSend.CreationDate = DateTime.UtcNow;
                }
            _raterPriceContext.SaveChanges();
                     
            return sendResult;                     
        }

        public CheckSmsStatusResult CheckAndUpdateSmsStatus(int smsSendId, string phone)
        {
            var checkSmsStatusResult = _smsGateWay.GetStatus(smsSendId, phone);
           
                var send = _raterPriceContext.SmsMessageSends.Where(s => s.Id == smsSendId).Select(s => s).First();
                send.ResponseToCheckStatusRequest = checkSmsStatusResult.RequestResult.TextView;
                if (checkSmsStatusResult.RequestResult.ErrorCode == null)
                {
                    send.Status = checkSmsStatusResult.Status;
                    send.StatusChangeDate = checkSmsStatusResult.LastStatusChangeDate;
                    send.SendDate = checkSmsStatusResult.SendDate;
                    send.Cost = checkSmsStatusResult.Cost;
                    //messageStatus.ErrorCode             
                }
                else
                {
                    send.ErrorCode = checkSmsStatusResult.RequestResult.ErrorCode; //????
                }
            _raterPriceContext.SaveChanges();
            
            return checkSmsStatusResult;
        }
    }
}
