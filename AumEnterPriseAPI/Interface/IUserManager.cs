using AumEnterPriseAPI.ViewModel;

namespace AumEnterPriseAPI.Interface
{
    public interface IUserManager
    {
        public UserViewModel GetUserByName(string username, string password);
        public UserViewModel GetUserById(int userId);
    }
}
