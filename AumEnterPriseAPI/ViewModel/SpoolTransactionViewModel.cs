namespace AumEnterPriseAPI.ViewModel
{
    public class SpoolTransactionViewModel
    {
        public long? SpoollD { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string? Day { get; set; }
        public int? SeriesNo { get; set; }
        public string? SubmittedDate { get; set; }
        public long SubClientID { get; set; }
        public string? SubClientName { get; set; }
        public long ClientID { get; set; }
        public string? ClientName { get; set; }
        public string? JobNo { get; set; }
        public string? PackTransmittalArea { get; set; }
        public string? Remarks { get; set; } = "";
        public short? YearID { get; set; } = 1;
        public int? TotalSpool { get; set; } = 0;
        public int SpoolingDone { get; set; } = 0;
        public int SpoolingBalance { get; set; } = 0;
        public int SubmissionBalance { get; set; } = 0;
        public short? ReasonID { get; set; }
        public List<SpoolControlSeriesViewModel> SpoolControlSeriesViewModels { get; set; } = new List<SpoolControlSeriesViewModel>();
    }

    public class SpoolControlSeriesViewModel
    {
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public int? From { get; set; }
        public int? To { get; set; }
    }
}
