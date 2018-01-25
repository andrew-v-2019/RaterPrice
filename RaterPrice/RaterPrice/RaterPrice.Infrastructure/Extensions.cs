namespace RaterPrice.Infrastructure
{
    public static class Extensions
    {
        public static string FormatPhoneNumber(string phone)
        {
            if (phone.IndexOf("+7") > -1)
            {
                phone = phone.Replace("+7", "7");
            }
            else
            {
                if (phone[0] == '8')
                {
                    phone = phone.Remove(0, 1);
                    phone = '7' + phone;
                }
                else
                {
                    if (phone.Length == 10 && phone[0] == '9')
                    {
                        phone = '7' + phone;
                    }
                }
            }
            return phone;
        }
    }
}
