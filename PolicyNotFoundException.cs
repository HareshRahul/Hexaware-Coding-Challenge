using System;

namespace InsuranceClaim.myexceptions
{
    // Custom exception for Policy not found
    public class PolicyNotFoundException : Exception
    {
        public PolicyNotFoundException() { }

        public PolicyNotFoundException(string message) : base(message) { }

        public PolicyNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
