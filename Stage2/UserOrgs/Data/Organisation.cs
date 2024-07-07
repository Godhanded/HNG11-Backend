namespace UserOrgs.Data
{
    public class Organisation
    {
        public string orgId { get; set; }
        public string name { get; set; }
        public string? description { get; set; }

        public ICollection<UserOrganisation> userOrganisations { get; set; }

    }
}
