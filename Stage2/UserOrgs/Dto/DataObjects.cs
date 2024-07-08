using System.ComponentModel.DataAnnotations;
using UserOrgs.Data;

namespace UserOrgs.Dto
{
    public record OrganisationDto(string? orgId, [Required] string name, string description)
    {
        public Organisation ToOrgModel()
        {
            return new()
            {
                orgId = Guid.NewGuid().ToString(),
                name = name,
                description = description
            };
        }
    };

    public record UserIdDto(string userId);
}
