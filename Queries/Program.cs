using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = new List<Movies>
            {
                new Movies{ Title = "The Dark Knight", Rating = 8.9f, Year = 2008 },
                new Movies{ Title = "Star Wars V", Rating = 8.0f, Year = 1980 },
                new Movies{ Title = "The King's Speech", Rating = 9.2f, Year = 2010 }
            };

            var query = movies.Where(y => y.Year > 2000);

            var enumerator = query.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }
        }
    }
}
