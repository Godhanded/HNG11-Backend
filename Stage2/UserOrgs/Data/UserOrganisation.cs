namespace UserOrgs.Data
{
    public class UserOrganisation
    {
        public User user { get; set; }
        public Organisation organisation { get; set; }

        public string userId { get; set; }
        public string organisationId { get; set; }
    }
}
