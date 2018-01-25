using System.Collections.Generic;

namespace RaterPrice.ViewModels.MobileApiModels.Registration
{
    public class SendSmsResultViewModel
    {
       public List<string> Errors { get; set; }
       public int? SmsStatus { get; set; }
       public string StatusInfo { get; set; }

       public int? SmsMessageSendId { get; set; }

        public int? UserId { get; set; }

        public string CodeTmp { get; set; }
    }
}
