using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceClaim.entity
{
    public class Client
    {
        public int clientId { get; set; }
        public string clientName { get; set; }
        public int policyId {  get; set; }
        
        public string contactInfo { get; set; }
        public string policy { get; set; }

        public Client() { }

        public Client(int clientId, string clientName, string contactInfo, string policy)
        {
            try
            {
                this.clientId = clientId;
                this.clientName = clientName;
                this.contactInfo = contactInfo;
                this.policy = policy;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating Client: " + ex.Message);
            }
        }

        public override string ToString()
        {
            return $"ClientId: {clientId}, Name: {clientName}, Contact: {contactInfo}, Policy: {policy}";
        }
    }
}
