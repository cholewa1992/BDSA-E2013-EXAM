using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Storage.EntityDto;

namespace WebServer
{
    public class UserDto : IEntityDto
    {
        public int Id { set; get; }
        public EntityState State { set; get; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }

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
