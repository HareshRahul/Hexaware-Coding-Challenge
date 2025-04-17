using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuranceClaim.entity;

namespace InsuranceClaim
{
    public interface IClaimDao
    {
        bool AddClaim(Claim claim);
        List<Claim> GetClaimsByClient(int clientId);
    }
}
