using AumEnterPriseAPI.ViewModel;

namespace AumEnterPriseAPI.Interface
{
    public interface IClientManager
    {
        public List<ClientViewModel> GetClients();
        public ClientViewModel GetClientById(long clientId);
        public bool AddClient(ClientViewModel clientViewModel, int insertedBy);
        public bool DeleteClientById(long clientId);
        public bool UpdateClientById(ClientViewModel clientViewModel, int updatedBy);
    }
}
