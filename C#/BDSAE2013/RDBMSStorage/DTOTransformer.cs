using System.Data.Entity.ModelConfiguration.Conventions;
using Storage.EntityDto;

namespace RDBMSStorage
{
    public static class DtoTransformer
    {
        public static TEntity Transform<TEntity>(Movies o) where TEntity : class, IEntityDto, new()
        {
            return new MovieDto
            {
                Title = o.Title,
                Kind = o.Kind,
                EpisodeNumber = o.EpisodeNumber,
                EpisodeOfId = o.EpisodeOf_Id,
                Id = o.Id,
                SeasonNumber = o.SeasonNumber,
                SeriesYear = o.SeriesYear,
                Year = o.Year + ""
            } as TEntity;
        }

        public static TEntity Transform<TEntity>(People o) where TEntity : class, IEntityDto, new()
        {
            return new PersonDto
            {
                Gender = o.Gender,
                Id = o.Id,
                Name = o.Name
            } as TEntity;
        }
    }
}
