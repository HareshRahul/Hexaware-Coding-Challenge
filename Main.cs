using System;
using System.Collections.Generic;
using InsuranceClaim.entity;
using InsuranceClaim.service;

namespace InsuranceClaim.mainmod
{
    class MainModule
    {
        static void Main(string[] args)
        {
            IPolicyService service = new InsuranceServiceImpl();
            bool exit = false;

            List<string> availablePolicyNames = new List<string> { "Health Plus", "Secure Life", "Care Cover", "Travel Guard"};
            List<string> availablePolicyTypes = new List<string> { "Health", "Life", "Vehicle", "Travel" };

            while (!exit)
            {
                try
                {
                    Console.WriteLine("\n--- Insurance Claim Management ---");
                    Console.WriteLine("0. Apply Policy");
                    Console.WriteLine("1. Create Policy");
                    Console.WriteLine("2. View All Policies");
                    Console.WriteLine("3. View Policy by ID");
                    Console.WriteLine("4. Update Policy");
                    Console.WriteLine("5. Delete Policy");
                    Console.WriteLine("6. Create Client");
                    Console.WriteLine("7. View Client by ID");
                    Console.WriteLine("0. Exit");
                    Console.Write("Enter your choice: ");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "0":
                            // Apply for Policy (Create Client and assign policy)
                            try
                            {
                                Console.WriteLine("\nAvailable Policy Names:");
                                foreach (var name in availablePolicyNames)
                                    Console.WriteLine($"{name}.");

                                Console.Write("Enter Policy Name: ");
                                string pname = Console.ReadLine();
                                if (!availablePolicyNames.Exists(name => name.ToLower() == pname.ToLower()))
                                {
                                    throw new InsuranceException("Policy name is invalid. Please choose a valid name from the list.");
                                    break;
                                }

                                Console.WriteLine("\nAvailable Policy Types:");
                                foreach (var type in availablePolicyTypes)
                                    Console.WriteLine($"- {type}");

                                Console.Write("Enter Policy Type: ");
                                string ptype = Console.ReadLine();
                                if (!availablePolicyTypes.Exists(type => type.ToLower() == ptype.ToLower()))
                                {
                                    Console.WriteLine("Invalid Policy Type. Choose from the list.");
                                    break;
                                }

                                Console.Write("Enter Coverage Amount (minimum of 1000): ");
                                if (decimal.TryParse(Console.ReadLine(), out decimal pamt))
                                {
                                    // Create policy
                                    Policy newPolicy = new Policy
                                    {
                                        PolicyName = pname,
                                        PolicyType = ptype,
                                        CoverageAmount = pamt
                                    };

                                    bool created = service.CreatePolicy(newPolicy);
                                    if (created)
                                    {
                                        Console.WriteLine("Policy created successfully.");

                                        // After policy is created, create a client and apply the policy
                                        Console.Write("Enter Client Name: ");
                                        string cname = Console.ReadLine();

                                        Console.Write("Enter Email: ");
                                        string email = Console.ReadLine();

                                        Console.Write("Enter Phone Number: ");
                                        string phone = Console.ReadLine();

                                        Client client = new Client
                                        {
                                            clientName = cname,
                                            contactInfo = email,
                                            policyId = newPolicy.PolicyId // Apply the created policy to the client
                                        };

                                        bool cstatus = service.CreateClient(client);
                                        Console.WriteLine(cstatus ? "Client created and policy applied." : "Failed to create client and apply policy.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to create policy.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid amount.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error while applying policy: {ex.Message}");
                            }
                            break;

                        case "1":
                            try
                            {
                                Console.WriteLine("\nAvailable Policy Names:");
                                foreach (var name in availablePolicyNames)
                                    Console.WriteLine($"{name}.");

                                Console.Write("Enter Policy Name: ");
                                string pname = Console.ReadLine();
                                if (!availablePolicyNames.Exists(name => name.ToLower() == pname.ToLower()))
                                {
                                    throw new InsuranceException("Policy name is invalid. Please choose a valid name from the list.");
                                    break;
                                }

                                Console.WriteLine("\nAvailable Policy Types:");
                                foreach (var type in availablePolicyTypes)
                                    Console.WriteLine($"- {type}");

                                Console.Write("Enter Policy Type: ");
                                string ptype = Console.ReadLine();
                                if (!availablePolicyTypes.Exists(type => type.ToLower() == ptype.ToLower()))
                                {
                                    Console.WriteLine("Invalid Policy Type. Choose from the list.");
                                    break;
                                }

                                Console.Write("Enter Coverage Amount (minimum of 1000): ");
                                if (decimal.TryParse(Console.ReadLine(), out decimal pamt))
                                {
                                    Policy newPolicy = new Policy
                                    {
                                        PolicyName = pname,
                                        PolicyType = ptype,
                                        CoverageAmount = pamt
                                    };
                                    bool created = service.CreatePolicy(newPolicy);
                                    Console.WriteLine(created ? "Policy created successfully." : "Failed to create policy.");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid amount.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error while creating policy: {ex.Message}");
                            }
                            break;

                        case "2":
                            try
                            {
                                List<Policy> allPolicies = service.GetAllPolicies();
                                Console.WriteLine("\n--- All Policies ---");
                                foreach (var p in allPolicies)
                                {
                                    Console.WriteLine($"ID: {p.PolicyId}, PolicyName: {p.PolicyName}, Type: {p.PolicyType}, Coverage: {p.CoverageAmount}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error while retrieving policies: {ex.Message}");
                            }
                            break;

                        case "3":
                            try
                            {
                                Console.Write("Enter Policy ID to view: ");
                                if (int.TryParse(Console.ReadLine(), out int searchId))
                                {
                                    Policy found = service.GetPolicy(searchId);
                                    if (found != null)
                                    {
                                        Console.WriteLine($"ID: {found.PolicyId}, PolicyName: {found.PolicyName}, PolicyType: {found.PolicyType}, Coverage: {found.CoverageAmount}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Policy not found.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid ID.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error while viewing policy: {ex.Message}");
                            }
                            break;

                        case "4":
                            try
                            {
                                Console.Write("Enter Policy ID to update: ");
                                if (int.TryParse(Console.ReadLine(), out int upId))
                                {
                                    var existingPolicy = service.GetPolicy(upId);
                                    if (existingPolicy == null)
                                    {
                                        Console.WriteLine("No policy found with the given ID.");
                                        break;
                                    }

                                    Console.WriteLine("Available Names:");
                                    availablePolicyNames.ForEach(name => Console.WriteLine($"- {name}"));
                                    Console.Write("Enter New Policy Name: ");
                                    string newName = Console.ReadLine();
                                    if (!availablePolicyNames.Exists(name => name.ToLower() == newName.ToLower()))
                                    {
                                        Console.WriteLine("Invalid name. Please select a valid policy name.");
                                        break;
                                    }

                                    Console.WriteLine("Available Types:");
                                    availablePolicyTypes.ForEach(type => Console.WriteLine($"- {type}"));
                                    Console.Write("Enter New Policy Type: ");
                                    string newType = Console.ReadLine();
                                    if (!availablePolicyTypes.Exists(type => type.ToLower() == newType.ToLower()))
                                    {
                                        Console.WriteLine("Invalid type. Please select a valid policy type.");
                                        break;
                                    }

                                    Console.Write("Enter New Coverage Amount: ");
                                    if (decimal.TryParse(Console.ReadLine(), out decimal newAmt))
                                    {
                                        Policy updated = new Policy
                                        {
                                            PolicyId = upId,
                                            PolicyName = newName,
                                            PolicyType = newType,
                                            CoverageAmount = newAmt
                                        };

                                        bool result = service.UpdatePolicy(updated);
                                        Console.WriteLine(result ? "Policy updated successfully." : "Policy update failed. Please try again.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid amount. Please enter a valid decimal value.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid ID. Please enter a valid numeric Policy ID.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error while updating policy: {ex.Message}");
                            }
                            break;

                        case "5":
                            try
                            {
                                Console.Write("Enter Policy ID to delete: ");
                                if (int.TryParse(Console.ReadLine(), out int delId))
                                {
                                    bool deleted = service.DeletePolicy(delId);
                                    Console.WriteLine(deleted ? "Policy deleted." : "Failed to delete policy.");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid ID.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error while deleting policy: {ex.Message}");
                            }
                            break;

                        case "6":
                            try
                            {
                                Console.Write("Enter Client Name: ");
                                string cname = Console.ReadLine();

                                Console.Write("Enter Email: ");
                                string email = Console.ReadLine();

                                Console.Write("Enter Phone Number: ");
                                string phone = Console.ReadLine();

                                Console.Write("Enter Policy ID to assign: ");
                                if (int.TryParse(Console.ReadLine(), out int cpid))
                                {
                                    Client client = new Client
                                    {
                                        clientName = cname,
                                        contactInfo = email,
                                        policyId = cpid
                                    };
                                    bool cstatus = service.CreateClient(client);
                                    Console.WriteLine(cstatus ? "Client created." : "Failed to create client.");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Policy ID.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error while creating client: {ex.Message}");
                            }
                            break;

                        case "7":
                            try
                            {
                                Console.Write("Enter Client ID: ");
                                if (int.TryParse(Console.ReadLine(), out int cid))
                                {
                                    Client c = service.GetClient(cid);
                                    if (c != null)
                                    {
                                        Console.WriteLine($"ID: {c.clientId}, Name: {c.clientName}, Email: {c.contactInfo}, PolicyID: {c.policyId}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Client not found.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid ID.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error while viewing client: {ex.Message}");
                            }
                            break;

                        case "9":
                            exit = true;
                            Console.WriteLine("Exiting...");
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (InsuranceException ie)
                {
                    Console.WriteLine($"Insurance Error: {ie.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"System Error: {ex.Message}");
                }
            }
        }
    }
}
