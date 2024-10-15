using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PoeSample.Models;

namespace PoeSample.Services
{
    public class ClaimService
    {
        ClaimsContext claimsContext;
        public ClaimService()
        {
            claimsContext = new ClaimsContext();
            claimsContext.Database.EnsureCreated();
        }

        public int AddNewClaim(Claim claim)
        {
            //logic to add to claim to you database

            //geting the lecturer defined rate per hour
            var rate = claimsContext.Rates.FirstOrDefault(x => x.PersonId == claim.PersonId);
            if (rate != null)
            {
                claim.Rate = rate.HourlyRate;
                double totalFee = claim.Hours * claim.Rate;
                claim.TotalFee = totalFee;
                claimsContext.Claims.Add(claim);
                claimsContext.SaveChanges();
                return claim.Id;
            }
            return 0;
        }

        public int UpdateClaim(Claim claim)
        {
            //logic to update to claim to you database
            var _claim = claimsContext.Claims.FirstOrDefault(x => x.Id == claim.Id);
            if (_claim != null)
            {
                double totalFee = claim.Hours * claim.Rate;
                _claim.TotalFee = totalFee;
                _claim.DateClaimed = claim.DateClaimed;
                _claim.ClassId = claim.ClassId;
                _claim.StatusId = claim.StatusId;
                //add fields
                claimsContext.SaveChanges();
            }
            return claim.Id;
        }

        public List<Claim> GetAllClaimsForUser(int personId)
        {
            // search on the db and return the user claims
            var claims = claimsContext.Claims.Where(x => x.PersonId == personId).ToList();

            return claims.OrderByDescending(x => x.DateClaimed).ThenBy(x => x.StatusId).ToList();
        }
    }
}
