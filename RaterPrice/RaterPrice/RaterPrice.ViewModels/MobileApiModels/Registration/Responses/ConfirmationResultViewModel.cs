namespace RaterPrice.ViewModels.MobileApiModels.Registration
{
    public class ConfirmationResultViewModel
    {
        public string Status { get; set; }
        public bool Confirmed { get; set; }
        public System.Guid? Token { get; set; }
    }
}
