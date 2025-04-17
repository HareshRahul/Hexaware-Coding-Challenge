using NUnit.Framework;
using InsuranceClaim.service;
using InsuranceClaim.entity;
using System;
using InsuranceClaim.exception;

namespace InsuranceClaim.Tests
{
    [TestFixture]
    public class InsuranceServiceTests
    {
        private IPolicyService _service;

        [SetUp]
        public void Setup()
        {
            // Initialize your service (use mocks if needed for database or other dependencies)
            _service = new InsuranceServiceImpl();
        }

        [Test]
        public void TestCreatePolicy_ShouldReturnTrue()
        {
            // Arrange
            var policy = new Policy
            {
                PolicyName = "Health Plus",
                PolicyType = "Health",
                CoverageAmount = 5000
            };

            // Act
            bool result = _service.CreatePolicy(policy);

            // Assert
            Assert.IsTrue(result, "Policy creation failed.");
        }

        
        [Test]
        public void TestGetPolicy_ShouldReturnNull_WhenInvalidId()
        {
            // Act
            Policy result = _service.GetPolicy(999);  // Assuming 999 is invalid ID

            // Assert
            Assert.IsNull(result, "Policy should be null for invalid ID.");
        }

    

        [Test]
        public void TestDeletePolicy_ShouldReturnFalse_WhenInvalidId()
        {
            // Arrange
            int policyIdToDelete = 999; // Assuming policy with ID 999 doesn't exist

            // Act
            bool result = _service.DeletePolicy(policyIdToDelete);

            // Assert
            Assert.IsFalse(result, "Policy deletion should fail for invalid ID.");
        }
    }
}
