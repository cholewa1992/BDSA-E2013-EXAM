using System;
using System.Text.RegularExpressions;

namespace Storage.EntityDto
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
                if (!Regex.IsMatch(value, @"."))
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
