//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Storage;

namespace EntityFrameworkStorage
{
    using System;
    using System.Collections.Generic;

    public partial class People : IEntityDto
    {
        public People()
        {
            this.Participate = new HashSet<Participate>();
            this.PersonInfo = new HashSet<PersonInfo>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
    
        public virtual ICollection<Participate> Participate { get; set; }
        public virtual ICollection<PersonInfo> PersonInfo { get; set; }
    }
}
