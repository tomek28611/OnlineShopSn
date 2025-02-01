using OnlineShop.Data.Entities;

namespace OnlineShop.Data.Dto
{
    public class BannerDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? SubTitle { get; set; }

        public string? ImageName { get; set; }

        public short? Priority { get; set; }

        public string? Link { get; set; }

        public string? Position { get; set; }

        public BannerEntity ToEntity()
        {
            return new BannerEntity { Id = Id, Title = Title, SubTitle = SubTitle, ImageName = ImageName, Priority = Priority, Link = Link };
        }
    }
}
