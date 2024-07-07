using System.ComponentModel.DataAnnotations;
using UserOrgs.Data;

namespace UserOrgs.Dto
{
    public class UserRegisterDto
    {

        [Required]
        [Length(1,60)]
        public string firstName { get; set; }
        [Required]
        [Length(1, 60)]
        public string lastName { get; set; }
        [Required,EmailAddress]
        public string email { get; set; }
        [Required,Length(2,60)]
        public string password { get; set; }

        [RegularExpression("^\\+?[1-500][0-9]{7,14}$", ErrorMessage = "Invalid Phone Number")]
        public string? phone { get; set; }

        public User ToModel()
        {
            return new User()
            {
                firstName = firstName,
                lastName = lastName,
                email = email,
                phone = phone
            };
        }
    }

}

