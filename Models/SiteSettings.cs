namespace DotNet_Header_Footer.Models
{
    public class SiteSettings
    {
        public int SettingId { get; set; }

        public string? BrandName { get; set; }

        public string? Logo { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? FacebookUrl { get; set; }

        public string? InstagramUrl { get; set; }

        public string? TwitterUrl { get; set; }

        public string? MapEmbedUrl { get; set; }

        public string? CopyrightText { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
