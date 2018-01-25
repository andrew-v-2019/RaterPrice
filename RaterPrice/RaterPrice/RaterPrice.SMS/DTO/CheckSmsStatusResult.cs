
using System;

namespace RaterPrice.SMS.DTO
{
    public class CheckSmsStatusResult
    {
        public RequestResult RequestResult { get; set; }

        public int? Status { get; set; }

        public string StatusText { get; set; }

        public DateTime? LastStatusChangeDate { get; set; }

        public DateTime? SendDate { get; set; }

        public bool Delivered { get; set; }

        public decimal? Cost { get; set; }

    }
}
