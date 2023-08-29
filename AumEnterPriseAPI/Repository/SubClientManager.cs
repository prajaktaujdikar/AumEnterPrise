using System.Data;
using AumEnterPriseAPI.AppCode;
using AumEnterPriseAPI.Interface;
using AumEnterPriseAPI.ViewModel;

namespace AumEnterPriseAPI.Repository
{
    public class SubClientManager : ISubClientManager
    {
        public IConfiguration _configuration;
        public SubClientManager(IConfiguration config)
        {
            _configuration = config;
        }

        public List<SubClientViewModel> GetSubClients()
        {
            List<SubClientViewModel> clientViewModels = new();
            using (SQLHelper db = new(_configuration))
            {
                using DataSet data = db.ExecDataSetProc("SubClientMasterGet");
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    clientViewModels.Add(new SubClientViewModel
                        {
                            SubClientID = Convert.ToInt64(row["SubClientID"]),
                            ClientID = Convert.ToInt64(row["ClientID"]),
                            SubClientName = row["SubClientName"].ToString(),
                            ClientName = row["ClientName"].ToString(),
                            EmailID = row["EmailID"].ToString(),
                            MobileNo = row["MobileNo"].ToString(),
                            Remarks = row["Remarks"].ToString()
                        }
                    );
                }
            }
            return clientViewModels;
        }

        public SubClientViewModel GetSubClientById(long subClientId)
        {
            SubClientViewModel clientViewModel = new();
            using (SQLHelper db = new(_configuration))
            {
                using DataSet dataSet = db.ExecDataSetProc("SubClientMasterByID", "@subClientId", subClientId);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0) {
                    DataRow row = dataSet.Tables[0].Rows[0];
                    clientViewModel = new SubClientViewModel
                    {
                        SubClientID = Convert.ToInt64(row["SubClientID"]),
                        ClientID = Convert.ToInt64(row["ClientID"]),
                        SubClientName = row["SubClientName"].ToString(),
                        ClientName = row["ClientName"].ToString(),
                        EmailID = row["EmailID"].ToString(),
                        MobileNo = row["MobileNo"].ToString(),
                        Remarks = row["Remarks"].ToString()
                    };
                }
            }
            return clientViewModel;
        }

        public List<SubClientViewModel> GetSubClientByClientId(long clientId)
        {
            List<SubClientViewModel> clientViewModels = new();
            using (SQLHelper db = new(_configuration))
            {
                using DataSet data = db.ExecDataSetProc("SubClientMasterByClientID", "@clientId", clientId);
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    clientViewModels.Add(new SubClientViewModel
                    {
                        SubClientID = Convert.ToInt64(row["SubClientID"]),
                        SubClientName = row["SubClientName"].ToString()
                    });
                }
            }
            return clientViewModels;
        }

        public bool AddSubClient(SubClientViewModel subClientViewModel, int insertedBy)
        {
            using (SQLHelper db = new(_configuration))
            {
                int rowsAffected = db.ExecNonQueryProc("SubClientMasterInsert", "@clientId", subClientViewModel.ClientID, "@subClientName", subClientViewModel.SubClientName, "@emailID", subClientViewModel.EmailID, "@mobileNo", subClientViewModel.MobileNo, "@remarks", subClientViewModel.Remarks ?? "", "InsertedBy", insertedBy, "InsertTime", DateTime.UtcNow);
                if (rowsAffected > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool DeleteSubClientById(long subClientId)
        {
            using (SQLHelper db = new(_configuration))
            {
                int rowsAffected = db.ExecNonQueryProc("SubClientMasterDelete", "@subClientId", subClientId);
                if (rowsAffected > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateSubClientById(SubClientViewModel subClientViewModel, int updatedBy)
        {
            using (SQLHelper db = new(_configuration))
            {
                int rowsAffected = db.ExecNonQueryProc("SubClientMasterUpdate", "@subClientId", subClientViewModel.SubClientID, "@clientId", subClientViewModel.ClientID, "@subClientName", subClientViewModel.SubClientName, "@emailID", subClientViewModel.EmailID, "@mobileNo", subClientViewModel.MobileNo, "@remarks", subClientViewModel.Remarks, "UpdatedBy", updatedBy, "UpdateTime", DateTime.UtcNow);
                if (rowsAffected > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
