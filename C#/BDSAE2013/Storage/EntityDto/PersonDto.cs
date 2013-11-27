using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Storage.EntityDto;

namespace WebServer
{
    public class PersonDto
    {
        public int Id { get; set; }
        public EntityState State { get; set; }
        
        private string _name;
        private string _gender;

        public string Name
        {
            get { return _name; }
            set
            {
                if (!Regex.IsMatch(value, @"[.]+"))
                {
                    throw new ArgumentException("A name can't be nothing");
                }
                _name = value;
            }
        }


        public string Gender
        {
            get { return _gender; }
            set
            {
                if (!Regex.IsMatch(value, @"[mMfF]"))
                {
                    throw new ArgumentException("gender must be m or M or f or F");
                }
                _gender = value;
            }
        }
    }
}
