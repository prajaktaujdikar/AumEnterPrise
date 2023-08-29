using System.Data;
using AumEnterPriseAPI.AppCode;
using AumEnterPriseAPI.Interface;
using AumEnterPriseAPI.ViewModel;
using Microsoft.Data.SqlClient;

namespace AumEnterPriseAPI.Repository
{
    public class SpoolTransactionManager : ISpoolTransactionManager
    {
        public IConfiguration _configuration;
        public SpoolTransactionManager(IConfiguration config)
        {
            _configuration = config;
        }

        public bool AddSpoolTransaction(SpoolTransactionViewModel spoolTransactionViewModel, int insertedBy)
        {
            long id = 0;
            int totalSpool = 0;
            foreach (SpoolControlSeriesViewModel controlSeriesViewModel in spoolTransactionViewModel.SpoolControlSeriesViewModels)
            {
                totalSpool += (controlSeriesViewModel.To - controlSeriesViewModel.From) + 1 ?? 0;
            }

            using (SqlConnection cn = new(_configuration.GetConnectionString("AumEnterPriseConnString")))
            {
                cn.Open();
                using (SqlCommand cmd = new("SpoolTransactionMasterInsert", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReceivedDate", spoolTransactionViewModel.ReceivedDate);
                    cmd.Parameters.AddWithValue("@SubClientID", spoolTransactionViewModel.SubClientID);
                    cmd.Parameters.AddWithValue("@JobNo", spoolTransactionViewModel.JobNo);
                    cmd.Parameters.AddWithValue("@PackTransmittalArea", spoolTransactionViewModel.PackTransmittalArea);
                    cmd.Parameters.AddWithValue("@Remarks", spoolTransactionViewModel.Remarks ?? "");
                    cmd.Parameters.AddWithValue("@YearID", spoolTransactionViewModel.YearID);
                    cmd.Parameters.AddWithValue("@TotalSpool", (spoolTransactionViewModel.TotalSpool ?? 0) + totalSpool);
                    cmd.Parameters.AddWithValue("@SpoolingDone", spoolTransactionViewModel.SpoolingDone);
                    cmd.Parameters.AddWithValue("@SpoolingBalance", spoolTransactionViewModel.SpoolingBalance);
                    cmd.Parameters.AddWithValue("@SubmissionBalance", spoolTransactionViewModel.SubmissionBalance);
                    cmd.Parameters.AddWithValue("@InsertedBy", insertedBy);
                    cmd.Parameters.AddWithValue("@InsertTime", DateTime.UtcNow);
                    var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();
                    id = (int)returnParameter.Value;
                }
            }

            if (id > 0)
            {
                return AddSpoolControlsSeries(spoolTransactionViewModel.SpoolControlSeriesViewModels, id, spoolTransactionViewModel.Remarks, insertedBy);
            }

            return false;
        }

        public bool AddSpoolControlsSeries(List<SpoolControlSeriesViewModel> spoolControlSeriesViewModel, long spoolID, string remarks, int insertedBy)
        {
            long id = 0;
            using (SqlConnection cn = new(_configuration.GetConnectionString("AumEnterPriseConnString")))
            {
                cn.Open();
                foreach (SpoolControlSeriesViewModel controlSeriesViewModel in spoolControlSeriesViewModel)
                {
                    using (SqlCommand cmd = new("SpoolControlsSeriesInsert", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SpoolID", spoolID);
                        cmd.Parameters.AddWithValue("@ToControlNo", controlSeriesViewModel.To);
                        cmd.Parameters.AddWithValue("@FromControlNo", controlSeriesViewModel.From);
                        cmd.Parameters.AddWithValue("@Prefix", controlSeriesViewModel.Prefix);
                        cmd.Parameters.AddWithValue("@Suffix", controlSeriesViewModel.Suffix);
                        cmd.Parameters.AddWithValue("@InsertedBy", insertedBy);
                        cmd.Parameters.AddWithValue("@InsertTime", DateTime.UtcNow);
                        var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        cmd.ExecuteNonQuery();
                        id = (int)returnParameter.Value;

                        if (id > 0)
                        {
                            bool isSuccess = AddSpoolControlNoDetail(controlSeriesViewModel, id, remarks, insertedBy);
                            if (!isSuccess)
                            {
                                return false;
                            }
                        }
                    }
                }
                //if (totalNoOfSpools > 0)
                //{
                //    SpoolControlSeriesViewModel manualSpoolControlSeriesViewModel = new SpoolControlSeriesViewModel();
                //    manualSpoolControlSeriesViewModel.From = 1;
                //    manualSpoolControlSeriesViewModel.To = totalNoOfSpools;
                //    using (SqlCommand cmd = new("SpoolControlsSeriesInsert", cn))
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@SpoolID", spoolID);
                //        cmd.Parameters.AddWithValue("@ToControlNo", manualSpoolControlSeriesViewModel.To);
                //        cmd.Parameters.AddWithValue("@FromControlNo", manualSpoolControlSeriesViewModel.From);
                //        cmd.Parameters.AddWithValue("@Prefix", "");
                //        cmd.Parameters.AddWithValue("@Suffix", "");
                //        cmd.Parameters.AddWithValue("@InsertedBy", insertedBy);
                //        cmd.Parameters.AddWithValue("@InsertTime", DateTime.UtcNow);
                //        var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                //        returnParameter.Direction = ParameterDirection.ReturnValue;

                //        cmd.ExecuteNonQuery();
                //        id = (int)returnParameter.Value;

                //        if (id > 0)
                //        {
                //            bool isSuccess = AddSpoolControlNoDetail(manualSpoolControlSeriesViewModel, id, remarks, insertedBy);
                //            if (!isSuccess)
                //            {
                //                return false;
                //            }
                //        }
                //    }
                //}
            }

            return true;
        }

        public bool AddSpoolControlNoDetail(SpoolControlSeriesViewModel spoolControlSeriesViewModel, long spoolControlsSeriesID, string remarks, int insertedBy)
        {
            using (SQLHelper db = new(_configuration))
            {
                for (int? i = spoolControlSeriesViewModel.From; i <= spoolControlSeriesViewModel.To; i++)
                {
                    db.ExecNonQueryProc("SpoolControlNoDetailInsert",
                        "@SpoolControlsSeriesID", spoolControlsSeriesID,
                        "@SpoolControlNo", spoolControlSeriesViewModel.Prefix + i + spoolControlSeriesViewModel.Suffix,
                        "@ReasonID", null,
                        "@Remark", remarks,
                        "@InsertedBy", insertedBy,
                        "@InsertTime", DateTime.UtcNow);
                }
            }
            return true;
        }

        public List<SpoolTransactionViewModel> GetAllSpoolTransactions()
        {
            List<SpoolTransactionViewModel> spoolTransactionViewModels = new();
            using (SQLHelper db = new(_configuration))
            {
                int i = 0;
                using DataSet data = db.ExecDataSetProc("SpoolTransactionMasterGet");
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    spoolTransactionViewModels.Add(new SpoolTransactionViewModel
                    {
                        SeriesNo = ++i,
                        SpoollD = Convert.ToInt64(row["SpoollD"]),
                        ClientName = row["ClientName"].ToString(),
                        SubClientName = row["SubClientName"].ToString(),
                        JobNo = row["JobNo"].ToString(),
                        PackTransmittalArea = row["PackTransmittalArea"].ToString(),
                        TotalSpool = Convert.ToInt32(row["TotalSpool"]),
                        Day = Convert.ToDateTime(row["ReceivedDate"]).ToString("dddd"),
                        SubmittedDate = Convert.ToDateTime(row["ReceivedDate"]).ToString("dd-MMM-yy"),
                    });
                }
            }
            return spoolTransactionViewModels;
        }
    }
}
