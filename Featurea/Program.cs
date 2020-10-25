using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Featurea
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int> squre = x => x * x;
            Func<int, int, int> add = (x, y) => x + y;
            Action<int> write = x => Console.WriteLine(x);

            write(squre(add(2, 3)));

            var developers = new Employee[]
            {
                new Employee { Id = 1, Name = "Anis" },
                new Employee { Id = 2, Name = "Jony" }
            };

            var sales = new List<Employee>()
            {
                new Employee { Id = 3, Name = "Alex" }
            };

            var query = developers.Where(e => e.Name.Length == 4)
                                   .OrderBy(e => e.Name);

            var query2 = from developer in developers
                         where developer.Name.Length <= 10
                         orderby developer.Name
                         select developer;

            foreach (var e in query2)
            {
                Console.WriteLine(e.Name);
            }
        }
    }
}
