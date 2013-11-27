using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.EntityDto;

namespace RDBMSStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            var sud = new RdbmsStorage();
            Console.WriteLine(
                sud.Get<MovieDto>(7523).Title);
                Console.ReadKey();
            
        }
    }
}
