using Storage;

namespace FlatFileStorage.Entities
{
    public class People : IEntityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
    }
}
