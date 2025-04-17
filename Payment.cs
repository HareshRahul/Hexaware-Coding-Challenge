using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace InsuranceClaim.entity
{
    public class Payment
    {
        public int paymentId { get; set; }
        public DateTime paymentDate { get; set; }
        public decimal paymentAmount { get; set; }
        public Client client { get; set; }

        public Payment() { }

        public Payment(int paymentId, DateTime paymentDate, decimal paymentAmount, Client client)
        {
            try
            {
                this.paymentId = paymentId;
                this.paymentDate = paymentDate;
                this.paymentAmount = paymentAmount;
                this.client = client;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating Payment: " + ex.Message);
            }
        }

        public override string ToString()
        {
            return $"PaymentId: {paymentId}, Date: {paymentDate.ToShortDateString()}, Amount: {paymentAmount}, Client: {client?.clientName}";
        }
    }
}
