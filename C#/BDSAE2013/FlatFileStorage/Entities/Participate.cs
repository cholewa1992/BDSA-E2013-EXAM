using Storage;

namespace FlatFileStorage.Entities
{
    public class Participate : IEntityDto
    {
        public int ParticipateId { get; set; }
        public int? NrOrder { get; set; }
        public string CharName { get; set; }
        public string Role { get; set; }
        public string Note { get; set; }
        public int? Movie_Id { get; set; }
        public int? Person_Id { get; set; }

        public int Id
        {
            set { ParticipateId = value; }
            get { return ParticipateId; }
        }
    }
}
