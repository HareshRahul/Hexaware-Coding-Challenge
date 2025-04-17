using System;
using System.Collections.Generic;
using InsuranceClaim.entity;
using InsuranceClaim.service;

namespace InsuranceClaim.mainmod
{
    class MainModule
    {
        // Enum for menu options
        enum MenuOption
        {
            ApplyPolicy = 0,
            CreatePolicy = 1,
            ViewAllPolicies = 2,
            ViewPolicyByID = 3,
            UpdatePolicy = 4,
            DeletePolicy = 5,
            CreateClient = 6,
            ViewClientByID = 7,
            Exit = 9
        }

        static void Main(string[] args)
        {
            IPolicyService service = new InsuranceServiceImpl();
            bool exit = false;

            List<string> availablePolicyNames = new List<string> { "Health Plus", "Secure Life", "Care Cover", "Travel Guard" };
            List<string> availablePolicyTypes = new List<string> { "Health", "Life", "Vehicle", "Travel" };

            while (!exit)
            {
                try
                {
                    Console.WriteLine("\n--- Insurance Claim Management ---");
                    Console.WriteLine($"{(int)MenuOption.ApplyPolicy}. Apply Policy");
                    Console.WriteLine($"{(int)MenuOption.CreatePolicy}. Create Policy");
                    Console.WriteLine($"{(int)MenuOption.ViewAllPolicies}. View All Policies");
                    Console.WriteLine($"{(int)MenuOption.ViewPolicyByID}. View Policy by ID");
                    Console.WriteLine($"{(int)MenuOption.UpdatePolicy}. Update Policy");
                    Console.WriteLine($"{(int)MenuOption.DeletePolicy}. Delete Policy");
                    Console.WriteLine($"{(int)MenuOption.CreateClient}. Create Client");
                    Console.WriteLine($"{(int)MenuOption.ViewClientByID}. View Client by ID");
                    Console.WriteLine($"{(int)MenuOption.Exit}. Exit");
                    Console.Write("Enter your choice: ");
                    string input = Console.ReadLine();

                    if (Enum.TryParse(input, out MenuOption selectedOption))
                    {
                        switch (selectedOption)
                        {
                            case MenuOption.ApplyPolicy:
                                // Apply for Policy
                                ApplyPolicy(service, availablePolicyNames, availablePolicyTypes);
                                break;

                            case MenuOption.CreatePolicy:
                                // Create Policy
                                CreatePolicy(service, availablePolicyNames, availablePolicyTypes);
                                break;

                            case MenuOption.ViewAllPolicies:
                                // View All Policies
                                ViewAllPolicies(service);
                                break;

                            case MenuOption.ViewPolicyByID:
                                // View Policy by ID
                                ViewPolicyByID(service);
                                break;

                            case MenuOption.UpdatePolicy:
                                // Update Policy
                                UpdatePolicy(service, availablePolicyNames, availablePolicyTypes);
                                break;

                            case MenuOption.DeletePolicy:
                                // Delete Policy
                                DeletePolicy(service);
                                break;

                            case MenuOption.CreateClient:
                                // Create Client
                                CreateClient(service);
                                break;

                            case MenuOption.ViewClientByID:
                                // View Client by ID
                                ViewClientByID(service);
                                break;

                            case MenuOption.Exit:
                                exit = true;
                                Console.WriteLine("Exiting...");
                                break;

                            default:
                                Console.WriteLine("Invalid choice.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
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

        // Methods for each action:

        static void ApplyPolicy(IPolicyService service, List<string> availablePolicyNames, List<string> availablePolicyTypes)
        {
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
                }

                Console.WriteLine("\nAvailable Policy Types:");
                foreach (var type in availablePolicyTypes)
                    Console.WriteLine($"- {type}");

                Console.Write("Enter Policy Type: ");
                string ptype = Console.ReadLine();
                if (!availablePolicyTypes.Exists(type => type.ToLower() == ptype.ToLower()))
                {
                    Console.WriteLine("Invalid Policy Type. Choose from the list.");
                    return;
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
        }

        static void CreatePolicy(IPolicyService service, List<string> availablePolicyNames, List<string> availablePolicyTypes)
        {
            // Similar structure to ApplyPolicy for creating a policy
        }

        static void ViewAllPolicies(IPolicyService service)
        {
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
        }
    }
}