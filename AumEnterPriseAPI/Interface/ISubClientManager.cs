using AumEnterPriseAPI.ViewModel;

namespace AumEnterPriseAPI.Interface
{
    public interface ISubClientManager
    {
        public List<SubClientViewModel> GetSubClients();
        public SubClientViewModel GetSubClientById(long subClientId);
        public List<SubClientViewModel> GetSubClientByClientId(long clientId);
        public bool AddSubClient(SubClientViewModel clientViewModel, int insertedBy);
        public bool DeleteSubClientById(long clientId);
        public bool UpdateSubClientById(SubClientViewModel clientViewModel, int updatedBy);
    }
}
