using UserOrgs.Dto;

namespace UserOrgs.Data
{

public class User
    {
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string passwordSalt { get; set; }
        public string? phone { get; set; }

        public ICollection<UserOrganisation> userOrganisations { get; set; }

        public UserDataDto ToUserDataDto()
        {
            return new()
            {
                userId = userId,
                email = email,
                firstName = firstName,
                lastName = lastName,
                phone = phone,

            };
        }
    }
}
