using System.Collections.Generic;


namespace Storage.EntityDto
{
    class FavoriteListDto : IEntityDto
    {
        public int Id { set; get; }
        public EntityState State { set; get; }

        private IList<MovieDto>  _favoriteMovies;
        
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
