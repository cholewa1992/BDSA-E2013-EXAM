using System;
using System.Text.RegularExpressions;

namespace Storage.EntityDto
{
    public class UserDto : IEntityDto
    {
        public int Id { set; get; }
        public EntityState State { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }
        public string Firstname { set; get; }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (!Regex.IsMatch(value, @"([a-zA-Z0-9]+)@([a-zA-Z0-9]+)\.([a-zA-Z]+)"))
                {
                    throw new ArgumentException("The email enteted was incorrect");
                }
                _email = value;
            }
        }

    }
}
