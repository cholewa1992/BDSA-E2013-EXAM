using System;
using System.Threading.Tasks;
using Storage;

namespace FlatFileStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var i = 0; i < 5; i++)
            {
                var i2 = i;
                var t = new Task(() => Fs(i2));
                t.Start();
                Console.WriteLine("Started " +i);
            }
            Console.ReadKey();
        }

        public static void Fs(int n)
        {
            while (true)
            {
                var fs = new InMemoryStorageConnection();

                var user = new UserAcc {Firstname = "Jacob"};
                fs.Add(user);

                fs.SaveChanges();

                int i = user.Id;
                #if DEBUG
                Console.WriteLine(n + ": " + i);
                #endif
                
                fs.Update(new UserAcc
                {
                    Id = i,
                    Firstname = "Cholewa"
                });

                fs.SaveChanges();

                fs.Delete(new UserAcc{Id = i});
                fs.SaveChanges();
            }
        }
    }
}
