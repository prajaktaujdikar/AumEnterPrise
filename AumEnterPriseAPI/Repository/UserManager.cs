using System.Data;
using AumEnterPriseAPI.AppCode;
using AumEnterPriseAPI.Interface;
using AumEnterPriseAPI.ViewModel;

namespace AumEnterPriseAPI.Repository
{
    public class UserManager : IUserManager
    {
        public IConfiguration _configuration;
        public UserManager(IConfiguration config)
        {
            _configuration = config;
        }

        public UserViewModel GetUserByName(string username, string password)
        {
            UserViewModel userViewModels = null;
            using (SQLHelper db = new(_configuration))
            {
                using DataSet dataSet = db.ExecDataSetProc("UserMasterCheckLogin", "@userName", username, "@password", password);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dataSet.Tables[0].Rows[0];
                    userViewModels = new UserViewModel
                    {
                        UserID = row["UserID"].ToString(),
                        FullName = row["FullName"].ToString(),
                        EmailID = row["EmailID"].ToString(),
                        UserName = row["UserName"].ToString(),
                        UserType = row["UserType"].ToString()
                    };
                }
            }
            return userViewModels;
        }

        public UserViewModel GetUserById(int userId)
        {
            UserViewModel userViewModels = null;
            using (SQLHelper db = new(_configuration))
            {
                using DataSet dataSet = db.ExecDataSetProc("UserMasterGetByID", "@userId", userId);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dataSet.Tables[0].Rows[0];
                    userViewModels = new UserViewModel
                    {
                        UserID = row["UserID"].ToString(),
                        FullName = row["FullName"].ToString(),
                        EmailID = row["EmailID"].ToString(),
                        UserName = row["UserName"].ToString(),
                        UserType = row["UserType"].ToString()
                    };
                }
            }
            return userViewModels;
        }
    }
}
