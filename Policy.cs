using System;

namespace InsuranceClaim.entity
{
    public class Policy
    {
        public int PolicyId { get; set; }
        public string PolicyName { get; set; }
        public string PolicyType { get; set; }
        public decimal CoverageAmount { get; set; }

        public Policy() { }

        public Policy(int policyId, string policyName, string policyType, decimal coverageAmount)
        {
            PolicyId = policyId;
            PolicyName = policyName;
            PolicyType = policyType;
            CoverageAmount = coverageAmount;
        }

        public override string ToString()
        {
            return $"PolicyId: {PolicyId}, PolicyName: {PolicyName}, PolicyType: {PolicyType}, CoverageAmount: {CoverageAmount}";
        }
    }
}
