using System;

namespace RaterPrice.ViewModels.MobileApi.Models
{
    public class ExecutionErrorDetails
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public Exception Exception { get; set; }
        public long ExceptionTraceToken { get; set; }
        public string FriendlyMessage { get; set; }
    }
}