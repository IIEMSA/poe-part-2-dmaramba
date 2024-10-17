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

        public List<ClaimItemModel> GetAllClaimsForUser(int personId)
        {
            // search on the db and return the user claims
            var claims = (from c in claimsContext.Claims
                          join p in claimsContext.People on c.PersonId equals p.Id
                          join s in claimsContext.ClaimStatuses on c.StatusId equals s.Id
                          join cl in claimsContext.Classes on c.ClassId equals cl.Id
                          join co in claimsContext.Courses on c.CourseId equals co.Id
                          where c.PersonId == personId
                          select new ClaimItemModel
                          {
                              Id = c.Id,
                              DateClaimed = c.DateClaimed,
                              ClassName = cl.ClassName,
                              CourseName = co.Title,
                              PersonName = p.FirstName + " " + p.LastName,
                              Rate = c.Rate,
                              Hours=c.Hours,
                              Status = s.Status,
                              StatusId = c.StatusId,
                              Total = c.TotalFee
                          }
                          ).OrderByDescending(x => x.DateClaimed).ToList();

            return claims.OrderByDescending(x => x.DateClaimed).ThenBy(x => x.StatusId).ToList();
        }
    }
}
