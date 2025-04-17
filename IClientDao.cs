using InsuranceClaim.entity;

namespace InsuranceClaim.dao
{
    public interface IClientDao
    {
        bool AddClient(Client client);
        Client GetClientById(int clientId);
        List<Client> GetAllClients();
    }
}
