using PoeSample.Models;

namespace PoeSample.Services
{
    public class DocumentService
    {
        ClaimsContext claimsContext;
        public DocumentService()
        {
            claimsContext = new ClaimsContext();
            claimsContext.Database.EnsureCreated();
        }

        //add new document
        public int AddClaimDocument(ClaimDocument claimDocument)
        {
            claimsContext.ClaimDocuments.Add(claimDocument);
            claimsContext.SaveChanges();
            return claimDocument.Id;
        }

        public void DeleteClaimDocument(int claimDocumentId)
        {
            var claimDocument = claimsContext.ClaimDocuments.FirstOrDefault(x => x.Id == claimDocumentId);
            if (claimDocument != null)
            {
                claimsContext.ClaimDocuments.Remove(claimDocument);
                claimsContext.SaveChanges();
            }
        }
        //get documents for a claim
        public List<ClaimDocument> GetClaimDocuments(int claimId)
        {
            var claimDocuments = claimsContext.ClaimDocuments.Where(x => x.ClaimId == claimId).ToList();
            return claimDocuments;
        }
    }
}
