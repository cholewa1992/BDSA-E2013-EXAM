using System;
using System.Threading.Tasks;
using InMemoryStorage;
using Storage;

namespace InMemoryStorageTestRun
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 0;
            for (var i = 0; i < 3; i++)
            {
                var t = new Task(() => Fs(n++));
                t.Start();
                Console.WriteLine("Started " +i);
            }
            Console.ReadKey();
        }

        public static void Fs(int n)
        {
            while (true)
            {
                using (var fs = new StorageConnectionBridgeFacade(new InMemoryStorageConnectionFactory()))
                {

                    var user = new UserAcc {Firstname = "Jacob"};
                    fs.Add(user);
                    int i = user.Id;
                    #if DEBUG
                    Console.WriteLine(n + ": " + i);
                    #endif

                    fs.Update(new UserAcc
                    {
                        Id = i,
                        Firstname = "Cholewa"
                    });

                    fs.Delete(new UserAcc {Id = i});
                }
            }
        }
    }
}
