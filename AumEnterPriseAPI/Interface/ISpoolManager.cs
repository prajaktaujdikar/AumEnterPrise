using AumEnterPriseAPI.ViewModel;

namespace AumEnterPriseAPI.Interface
{
    public interface ISpoolTransactionManager
    {
        public bool AddSpoolTransaction(SpoolTransactionViewModel spoonViewModel, int insertedBy);
        public List<SpoolTransactionViewModel> GetAllSpoolTransactions();
    }
}
