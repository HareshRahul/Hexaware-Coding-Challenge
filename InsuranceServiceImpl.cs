using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InsuranceClaim.dao;
using InsuranceClaim.entity;

namespace InsuranceClaim.service
{
    public class InsuranceServiceImpl : IPolicyService
    {
        private SqlConnection conn;
        private object customerName;

        // --- POLICY METHODS ---

        public bool CreatePolicy(Policy policy)
        {
            try
            {
                // Only allow valid policy types
                List<string> validTypes = new List<string> { "Health", "Life", "Vehicle" };

                if (policy.PolicyType != "Health" && policy.PolicyType != "Life")
                {
                    throw new ArgumentException("Invalid Policy Type");
                }

                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "INSERT INTO Policies (policyName, policyType, coverageAmount) VALUES (@name, @type, @amount)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", policy.PolicyName);
                    cmd.Parameters.AddWithValue("@type", policy.PolicyType);
                    cmd.Parameters.AddWithValue("@amount", policy.CoverageAmount);
                    int rows = cmd.ExecuteNonQuery();

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CreatePolicy: " + ex.Message);
                return false;
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
            using (SqlConnection conn = DBUtility.GetConnection())
                if(conn == null) return false;
            try
            {
                
                {
                    // Step 1: Get all clientIds for this policy
                    SqlCommand getClientIdsCmd = new SqlCommand("SELECT clientId FROM Clients WHERE policyId = @policyId", conn);
                    getClientIdsCmd.Parameters.AddWithValue("@policyId", policyId);

                    List<int> clientIds = new List<int>();
                    using (SqlDataReader reader = getClientIdsCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientIds.Add(reader.GetInt32(0));
                        }
                    }

                    // Step 2: Delete claims for those clients
                    foreach (int clientId in clientIds)
                    {
                        SqlCommand deleteClaimsCmd = new SqlCommand("DELETE FROM Claims WHERE clientId = @clientId", conn);
                        deleteClaimsCmd.Parameters.AddWithValue("@clientId", clientId);
                        deleteClaimsCmd.ExecuteNonQuery();
                    }

                    // Step 3: Delete clients linked to policy
                    SqlCommand deleteClientsCmd = new SqlCommand("DELETE FROM Clients WHERE policyId = @policyId", conn);
                    deleteClientsCmd.Parameters.AddWithValue("@policyId", policyId);
                    deleteClientsCmd.ExecuteNonQuery();

                    // Step 4: Delete the policy
                    SqlCommand deletePolicyCmd = new SqlCommand("DELETE FROM Policies WHERE policyId = @policyId", conn);
                    deletePolicyCmd.Parameters.AddWithValue("@policyId", policyId);
                    int rowsAffected = deletePolicyCmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeletePolicy: " + ex.Message);
                return false;
            }
        }
        public bool ApplyPolicy(int clientId, int policyId)
        {
            using (SqlConnection connection = DBUtility.GetConnection())
            {
                if (connection == null) return false;

                try
                {
                    // Step 1: Check if the policy exists
                    string checkPolicyQuery = "SELECT COUNT(1) FROM Policies WHERE policyId = @policyId";
                    SqlCommand checkPolicyCmd = new SqlCommand(checkPolicyQuery, connection);
                    checkPolicyCmd.Parameters.AddWithValue("@policyId", policyId);

                    int policyExists = (int)checkPolicyCmd.ExecuteScalar();
                    if (policyExists == 0)
                    {
                        Console.WriteLine("Policy does not exist.");
                        return false;
                    }

                    // Step 2: Check if the client already has a policy applied
                    string checkClientPolicyQuery = "SELECT COUNT(1) FROM Clients WHERE clientId = @clientId";
                    SqlCommand checkClientPolicyCmd = new SqlCommand(checkClientPolicyQuery, connection);
                    checkClientPolicyCmd.Parameters.AddWithValue("@clientId", clientId);

                    int clientHasPolicy = (int)checkClientPolicyCmd.ExecuteScalar();
                    if (clientHasPolicy > 0)
                    {
                        Console.WriteLine("Client already has a policy applied.");
                        return false;
                    }

                    // Step 3: Apply the policy to the client by inserting the relationship
                    string applyPolicyQuery = "UPDATE Clients SET policyId = @policyId WHERE clientId = @clientId";
                    SqlCommand applyPolicyCmd = new SqlCommand(applyPolicyQuery, connection);
                    applyPolicyCmd.Parameters.AddWithValue("@clientId", clientId);
                    applyPolicyCmd.Parameters.AddWithValue("@policyId", policyId);

                    int rowsAffected = applyPolicyCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Policy {policyId} successfully applied to Client {clientId}.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Failed to apply policy.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in ApplyPolicy: {ex.Message}");
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
