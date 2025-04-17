using InsuranceClaim.entity;

namespace InsuranceClaim.dao
{
    public interface IPaymentDao
    {
        bool AddPayment(Payment payment);
        List<Payment> GetPaymentsByClient(int clientId);
    }
}
