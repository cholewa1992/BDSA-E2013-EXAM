using Storage;

namespace FlatFileStorage.Entities
{
    public class FavouriteList : IEntityDto
    {
        public int Id { get; set; }
        public int UserAccId { get; set; }
        public string Title { get; set; }
    }
}
