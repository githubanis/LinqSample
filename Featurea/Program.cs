using System;
using System.Collections.Generic;

namespace Featurea
{
    class Program
    {
        static void Main(string[] args)
        {
            var developers = new Employee[]
            {
                new Employee { Id = 1, Name = "Anis" },
                new Employee { Id = 2, Name = "Jony" }
            };

            var sales = new List<Employee>()
            {
                new Employee { Id = 3, Name = "Alex" }
            };

            foreach (var person in sales)
            {
                Console.WriteLine(person.Name);
            }
        }
    }
}
