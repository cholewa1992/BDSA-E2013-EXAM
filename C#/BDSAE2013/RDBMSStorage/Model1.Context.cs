﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class fakeimdbEntities : DbContext
    {
        public fakeimdbEntities()
            : base("name=fakeimdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<InfoType> InfoType { get; set; }
        public DbSet<MovieInfo> MovieInfo { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Participate> Participate { get; set; }
        public DbSet<People> People { get; set; }
        public DbSet<PersonInfo> PersonInfo { get; set; }
    }
}
