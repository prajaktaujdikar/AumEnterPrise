using System.Data;
using AumEnterPriseAPI.AppCode;
using AumEnterPriseAPI.Interface;
using AumEnterPriseAPI.ViewModel;

namespace AumEnterPriseAPI.Repository
{
    public class ClientManager : IClientManager
    {
        public IConfiguration _configuration;
        public ClientManager(IConfiguration config)
        {
            _configuration = config;
        }

        public List<ClientViewModel> GetClients()
        {
            List<ClientViewModel> clientViewModels = new();
            using (SQLHelper db = new(_configuration))
            {
                using DataSet data = db.ExecDataSetProc("ClientMasterGet");
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    clientViewModels.Add(new ClientViewModel
                        {
                            ClientID = Convert.ToInt64(row["ClientID"]),
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

        public ClientViewModel GetClientById(long clientId)
        {
            ClientViewModel clientViewModel = new();
            using (SQLHelper db = new(_configuration))
            {
                using DataSet dataSet = db.ExecDataSetProc("ClientMasterByID", "@clientId", clientId);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0) {
                    DataRow row = dataSet.Tables[0].Rows[0];
                    clientViewModel = new ClientViewModel
                    {
                        ClientID = Convert.ToInt64(row["ClientID"]),
                        ClientName = row["ClientName"].ToString(),
                        EmailID = row["EmailID"].ToString(),
                        MobileNo = row["MobileNo"].ToString(),
                        Remarks = row["Remarks"].ToString()
                    };
                }
            }
            return clientViewModel;
        }

        public bool AddClient(ClientViewModel clientViewModel, int insertedBy)
        {
            using (SQLHelper db = new(_configuration))
            {
                int rowsAffected = db.ExecNonQueryProc("ClientMasterInsert", "@clientName", clientViewModel.ClientName, "@emailID", clientViewModel.EmailID, "@mobileNo", clientViewModel.MobileNo, "@remarks", clientViewModel.Remarks ?? "", "InsertedBy", insertedBy, "InsertTime", DateTime.UtcNow);
                if (rowsAffected > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool DeleteClientById(long clientId)
        {
            using (SQLHelper db = new(_configuration))
            {
                int rowsAffected = db.ExecNonQueryProc("ClientMasterDelete", "@clientId", clientId);
                if (rowsAffected > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateClientById(ClientViewModel clientViewModel, int updatedBy)
        {
            using (SQLHelper db = new(_configuration))
            {
                int rowsAffected = db.ExecNonQueryProc("ClientMasterUpdate", "@clientId", clientViewModel.ClientID, "@clientName", clientViewModel.ClientName, "@emailID", clientViewModel.EmailID, "@mobileNo", clientViewModel.MobileNo, "@remarks", clientViewModel.Remarks, "UpdatedBy", updatedBy, "UpdateTime", DateTime.UtcNow);
                if (rowsAffected > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
