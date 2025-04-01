namespace Crypto.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; }

        public List<SocialMedia> SocialMedias{ get; set; }
    }

    public class SocialMedia
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public int Url { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
