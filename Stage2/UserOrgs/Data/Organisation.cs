using System.Numerics;
using UserOrgs.Dto;

namespace UserOrgs.Data
{
    public class Organisation
    {
        public string orgId { get; set; }
        public string name { get; set; }
        public string? description { get; set; }

        public ICollection<User> users { get; set; } = [];

        //public ICollection<UserOrganisation> userOrganisations { get; set; }

        public OrganisationDto ToOrgDataDto()
        {
            return new(orgId, name, description);
           
        }

    }
}
