using System.ComponentModel.DataAnnotations;

namespace UserOrgs.Dto
{
    public class UserDataDto
    {
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string? phone { get; set; }
    }

    
}
