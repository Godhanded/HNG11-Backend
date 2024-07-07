using System.ComponentModel.DataAnnotations;

namespace UserOrgs.Dto
{
    public record UserLoginDto([EmailAddress]string email, [Required]string password);
}
