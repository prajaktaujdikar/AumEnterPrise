namespace AumEnterPriseAPI.ViewModel
{
    public class SubClientViewModel
    {
        public long SubClientID { get; set; }
        public long ClientID { get; set; }
        public string? ClientName { get; set; }
        public string SubClientName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string? Remarks { get; set; } = "";
    }
}
