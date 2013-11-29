using System;
using System.Linq;
using Storage;

namespace EntityFrameworkStorage
{
    class TestRun
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Now adding a user");
            using (var db = new EFConnectionFactory().GetConnection())
            {
                var user = new UserAcc
                {
                    Email = "jbec@itu.dk",
                    Firstname = "Jacob",
                    Lastname = "Cholewa",
                    Password = "1234",
                    Username = "RAzor"
                };
                db.Add(user);
                Console.WriteLine(db.SaveChanges());
            }
            Console.WriteLine("Press any key...");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Now updating the user");
            using (var db = new EFConnectionFactory().GetConnection())
            {
                var user = db.Get<UserAcc>().First();
                user.Password = "12345";
                db.Update(user);
                Console.WriteLine(db.SaveChanges());
            }
            Console.WriteLine("Press any key...");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Now deleteing the user");
            using (var db = new EFConnectionFactory().GetConnection())
            {
                var user = db.Get<UserAcc>().First();
                db.Delete(user);
                Console.WriteLine(db.SaveChanges());
            }
            Console.WriteLine("Press any key...");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Now counting all users contaning the letter A");
            using (var db = new EFConnectionFactory().GetConnection())
            {
                var start = DateTime.Now;
                Console.WriteLine(db.Get<Movies>().Count(t => t.Title.Contains("A")));
                Console.WriteLine("- ended in " + (DateTime.Now - start).TotalMilliseconds + "ms");
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
