using NUnit.Framework;
using InsuranceClaim.entity;
using InsuranceClaim.service;

namespace InsuranceClaim.Tests
{
    [TestFixture]
    public class InsuranceClaimTests
    {
        private IPolicyService _policyService;

        [SetUp]
        public void Setup()
        {
            
            _policyService = new InsuranceServiceImpl(); // Assuming this class exists
        }

        [Test]
        public void Test_CreatePolicy_Success()
        {
            // Arrange
            var newPolicy = new Policy
            {
                PolicyName = "Health Plus",
                PolicyType = "Health",
                CoverageAmount = 5000.00m
            };

            // Act
            bool result = _policyService.CreatePolicy(newPolicy);

            // Assert
            Assert.IsTrue(result, "Policy should be created successfully.");
        }

        
        
    }
}
