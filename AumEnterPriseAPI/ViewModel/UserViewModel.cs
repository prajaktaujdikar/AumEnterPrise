namespace AumEnterPriseAPI.ViewModel
{
    public class UserViewModel
    {
        public string UserID { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
    }

    public class UserLoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
