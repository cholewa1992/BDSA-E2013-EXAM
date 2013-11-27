using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Storage.EntityDto;


namespace WebServer
{
   public class MovieDto : IEntityDto
    {
        public int Id { set; get; }
        public EntityState State { set; get; }
        public string Title { set; get; }
        public string Kind { set; get; }
        public int? SeasonNumber { set; get; }
        public int? EpisodeNumber { set; get; }
        public int? EpisodeOfId { set; get; }
        
        
        private string _year;
        private string _seriesYear;

        
        public string Year
        {
            get { return _year; }
            set
            {
                if (value == null || !Regex.IsMatch(value, @"^\d{4}$"))
                {
                    throw new ArgumentException("Year consist of 4 numbers. ex: 2013");
                }
                _year = value;
            }
        }

       
        public string SeriesYear
        {
            get { return _seriesYear; }
            set
            {
                //if (!Regex.IsMatch(value, @"^\d{4}$"))
                //{
                //    throw new ArgumentException("Year consist of numbers. ex: 2013");
                //}
                _seriesYear = value;
            }

        }
    }
}
