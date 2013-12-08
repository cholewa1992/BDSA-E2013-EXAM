using Storage;

namespace FlatFileStorage.Entities
{
    public class PersonInfo : IEntityDto
    {
        public int PersonInfoId { get; set; }
        public string Info { get; set; }
        public string Note { get; set; }
        public int Person_Id { get; set; }
        public int Type_Id { get; set; }

        public int Id
        {
            set { PersonInfoId = value; }
            get { return PersonInfoId; }
        }
    }
}
