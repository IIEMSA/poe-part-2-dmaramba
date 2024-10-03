using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PoeSample.Models;

namespace PoeSample.Services
{
    public class ClaimService
    {
        public ClaimService() { }

        public int AddNewClaim(Claim claim)
        {
            //logic to add to claim to you database

            return claim.Id;
        }

        public int UpdateClaim(Claim claim)
        {
            //logic to update to claim to you database

            return claim.Id;
        }

        public List<Claim> GetAllClaimsForUser(int personId)
        {
            // search on the db and return the user claims
            return new List<Claim>();
        }
    }
}
