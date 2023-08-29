namespace AumEnterPriseAPI.ViewModel
{
    public class ClientViewModel
    {
        public long ClientID { get; set; }
        public string ClientName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string? Remarks { get; set; } = "";
    }
}
