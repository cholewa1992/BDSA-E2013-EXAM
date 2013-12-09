using Storage;

namespace FlatFileStorage.Entities
{
    public class MovieInfo : IEntityDto
    {
        public int MovieInfoId { get; set; }
        public string Info { get; set; }
        public string Note { get; set; }
        public int Movie_Id { get; set; }
        public int? Type_Id { get; set; }

        public int Id
        {
            set { MovieInfoId = value; }
            get { return MovieInfoId; }
        }
    }
}
