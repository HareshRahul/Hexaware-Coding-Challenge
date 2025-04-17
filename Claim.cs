using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace InsuranceClaim.entity
{
    public class Claim
    {
        public int claimId { get; set; }
        public string claimNumber { get; set; }
        public DateTime dateFiled { get; set; }
        public decimal claimAmount { get; set; }
        public string status { get; set; }
        public int policyId { get; set; }
        public Client client { get; set; }

        public Claim() { }

        public Claim(int claimId, string claimNumber, DateTime dateFiled, decimal claimAmount, string status, string policy, Client client)
        {
            try
            {
                this.claimId = claimId;
                this.claimNumber = claimNumber;
                this.dateFiled = dateFiled;
                this.claimAmount = claimAmount;
                this.status = status;
                this.policyId = policyId;
                this.client = client;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating Claim: " + ex.Message);
            }
        }

        public override string ToString()
        {
            return $"ClaimId: {claimId}, Number: {claimNumber}, Date: {dateFiled.ToShortDateString()}, Amount: {claimAmount}, Status: {status}, PolicyId: {policyId}, Client: {client?.clientName}";
        }
    }
}
