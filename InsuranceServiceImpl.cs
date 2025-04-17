using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InsuranceClaim.dao;
using InsuranceClaim.entity;

namespace InsuranceClaim.service
{
    public class InsuranceServiceImpl : IPolicyService
    {
        // --- POLICY METHODS ---

        public bool CreatePolicy(Policy policy)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                if (connection == null) return false;

                try
                {
                    string query = "INSERT INTO Policies (policyName, policyType, coverageAmount) VALUES (@policyName, @policyType, @coverageAmount)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@policyName", policy.PolicyName);
                    cmd.Parameters.AddWithValue("@policyType", policy.PolicyType);
                    cmd.Parameters.AddWithValue("@coverageAmount", policy.CoverageAmount);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in CreatePolicy: {ex.Message}");
                    return false;
                }
            }
        }

        public Policy GetPolicy(int policyId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                if (connection == null) return null;

                try
                {
                    string query = "SELECT * FROM Policies WHERE policyId = @policyId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@policyId", policyId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Policy
                        {
                            PolicyId = (int)reader["policyId"],
                            PolicyName = reader["policyName"].ToString(),
                            PolicyType = reader["policyType"].ToString(),
                            CoverageAmount = (decimal)reader["coverageAmount"]
                        };
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetPolicy: {ex.Message}");
                    return null;
                }
            }
        }

        public List<Policy> GetAllPolicies()
        {
            List<Policy> policies = new List<Policy>();
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                if (connection == null) return policies;

                try
                {
                    string query = "SELECT * FROM Policies";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        policies.Add(new Policy
                        {
                            PolicyId = (int)reader["policyId"],
                            PolicyName = reader["policyName"].ToString(),
                            PolicyType = reader["policyType"].ToString(),
                            CoverageAmount = (decimal)reader["coverageAmount"]
                        });
                    }
                    return policies;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetAllPolicies: {ex.Message}");
                    return policies;
                }
            }
        }

        public bool UpdatePolicy(Policy policy)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                if (connection == null) return false;

                try
                {
                    string query = "UPDATE Policies SET policyName = @policyName, policyType = @policyType, coverageAmount = @coverageAmount WHERE policyId = @policyId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@policyId", policy.PolicyId);
                    cmd.Parameters.AddWithValue("@policyName", policy.PolicyName);
                    cmd.Parameters.AddWithValue("@policyType", policy.PolicyType);
                    cmd.Parameters.AddWithValue("@coverageAmount", policy.CoverageAmount);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in UpdatePolicy: {ex.Message}");
                    return false;
                }
            }
        }

        public bool DeletePolicy(int policyId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                if (connection == null) return false;

                try
                {
                    string query = "DELETE FROM Policies WHERE policyId = @policyId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@policyId", policyId);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in DeletePolicy: {ex.Message}");
                    return false;
                }
            }
        }

        // --- CLAIM METHODS ---

        public bool CreateClaim(Claim claim)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                if (connection == null) return false;

                try
                {
                    string query = "INSERT INTO Claims (claimDate, claimAmount, policyId) VALUES (@claimDate, @claimAmount, @policyId)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@claimDate", claim.dateFiled);
                    cmd.Parameters.AddWithValue("@claimAmount", claim.claimAmount);
                    cmd.Parameters.AddWithValue("@policyId", claim.policyId);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in CreateClaim: {ex.Message}");
                    return false;
                }
            }
        }

        public List<Claim> GetClaimsByPolicyId(int policyId)
        {
            List<Claim> claims = new List<Claim>();
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                if (connection == null) return claims;

                try
                {
                    string query = "SELECT * FROM Claims WHERE policyId = @policyId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@policyId", policyId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        claims.Add(new Claim
                        {
                            claimId = (int)reader["claimId"],
                            
                            claimAmount = (decimal)reader["claimAmount"],
                            policyId = (int)reader["policyId"]
                        });
                    }
                    return claims;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetClaimsByPolicyId: {ex.Message}");
                    return claims;
                }
            }
        }

        // --- CLIENT METHODS ---

        public bool CreateClient(Client client)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                if (connection == null) return false;

                try
                {
                    string query = "INSERT INTO Clients (clientName, contactInfo, policyId) VALUES (@clientName, @contactInfo, @policyId)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@clientName", client.clientName);
                    cmd.Parameters.AddWithValue("@contactInfo", client.contactInfo);
                    cmd.Parameters.AddWithValue("@policyId", client.policyId);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in CreateClient: {ex.Message}");
                    return false;
                }
            }
        }

        public Client GetClient(int clientId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                if (connection == null) return null;

                try
                {
                    string query = "SELECT * FROM Clients WHERE clientId = @clientId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@clientId", clientId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Client
                        {
                            clientId = (int)reader["clientId"],
                            clientName = reader["clientName"].ToString(),
                            contactInfo = reader["contactInfo"].ToString(),
                            policyId = (int)reader["policyId"]
                        };
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetClient: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
