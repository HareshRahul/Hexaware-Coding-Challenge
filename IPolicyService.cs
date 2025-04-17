using InsuranceClaim.entity;
using System;
using System.Collections.Generic;

namespace InsuranceClaim.service
{
    public interface IPolicyService
    {
        // Policy-related methods
        bool CreatePolicy(Policy policy);
        Policy GetPolicy(int policyId);
        List<Policy> GetAllPolicies();
        bool UpdatePolicy(Policy policy);
        bool DeletePolicy(int policyId);

        // Claim-related methods
        bool CreateClaim(Claim claim);
        List<Claim> GetClaimsByPolicyId(int policyId);

        // Client-related methods
        bool CreateClient(Client client);
        Client GetClient(int clientId);

        // Apply Policy to Client
        bool ApplyPolicy(int clientId, int policyId); // Apply a policy to a client
    }
}
