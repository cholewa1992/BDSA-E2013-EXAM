using System.Collections.Generic;


namespace Storage.EntityDto
{
    class FavoriteListDto : IEntityDto
    {
        public int Id { set; get; }
        public EntityState State { set; get; }

        // Type shall be changed to List<Movie>.
        private IList<MovieDto>  _favoriteMovies;
        
        // Type shall be changed to List<Movie>
        public IList<MovieDto> FavoriteMovie
        {
            set; get;
        }

        public void AddToFavourite(MovieDto movie)
        {
            FavoriteMovie.Add(movie);
        }
    }
}
