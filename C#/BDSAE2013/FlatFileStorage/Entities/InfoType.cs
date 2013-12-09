using Storage;

namespace FlatFileStorage.Entities
{
    public class InfoType : IEntityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
