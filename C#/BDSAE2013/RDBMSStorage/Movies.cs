//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RDBMSStorage
{
    using System;
    using System.Collections.Generic;
    
    public partial class Movies
    {
        public Movies()
        {
            this.MovieInfo = new HashSet<MovieInfo>();
            this.Movies1 = new HashSet<Movies>();
            this.Participate = new HashSet<Participate>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Kind { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> SeasonNumber { get; set; }
        public Nullable<int> EpisodeNumber { get; set; }
        public string SeriesYear { get; set; }
        public Nullable<int> EpisodeOf_Id { get; set; }
    
        public virtual ICollection<MovieInfo> MovieInfo { get; set; }
        public virtual ICollection<Movies> Movies1 { get; set; }
        public virtual Movies Movies2 { get; set; }
        public virtual ICollection<Participate> Participate { get; set; }
    }
}