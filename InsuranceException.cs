
namespace InsuranceClaim.mainmod
{
    [Serializable]
    internal class InsuranceException : Exception
    {
        public InsuranceException()
        {
        }

        public InsuranceException(string? message) : base(message)
        {
        }

        public InsuranceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}