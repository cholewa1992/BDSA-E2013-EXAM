using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.EntityDto;


namespace Storage.EntityDto
{
    class FavoriteListDto
    {
        public int Id { set; get; }
        public EntityState State { set; get; }

        // Type shall be changed to List<Movie>.
        private List<Object>  _favoriteMovies;
        
        // Type shall be changed to List<Movie>
        public List<Object> FavoriteMovie
        {
            set
            {
                _favoriteMovies.Add(value);
            }
        }
    }
}
