using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");
            var mannufacturers = ProcessManufacturers("manufacturers.csv");

            var query2 =
                cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                    .OrderByDescending(c => c.Combined)
                    .ThenBy(c => c.Name)
                    .Select(c => c);

            var query =
                from car in cars
                where car.Manufacturer == "BMW" && car.Year == 2016
                orderby car.Combined, car.Name
                select new
                {
                    car.Manufacturer,
                    car.Name,
                    car.Combined
                };

            //var result = cars.SelectMany(c => c.Name)
            //                 .OrderBy(c => c);

            //foreach (var character in result)
            //{
            //    Console.WriteLine(character);
            //}

            foreach (var car in query2.Take(10))
            {
                Console.WriteLine($"{car.Manufacturer} : {car.Name} : {car.Combined}");
            }
        }

        private static List<Car> ProcessFile(string path)
        {
            var query = 
                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(line => line.Length > 1)
                    .ToCar();

            //var query =  from line in File.ReadAllLines(path).Skip(1)
            //             where line.Length > 1
            //             select Car.ParseFromCsv(line);

            return query.ToList();
        }

        private static List<Manufacturers> ProcessManufacturers(string path)
        {
            var query =
                    File.ReadAllLines(path)
                        .Where(l => l.Length > 1)
                        .Select(l =>
                        {
                            var columns = l.Split(',');
                            return new Manufacturers
                            {
                                Name = columns[0],
                                Headquarters = columns[1],
                                Year = int.Parse(columns[2])
                            };
                        });
            return query.ToList();
        }
    }

    public static class CarExtension
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }
}
