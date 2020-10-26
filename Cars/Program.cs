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
            var cars             = ProcessFile("fuel.csv");
            var mannufacturers   = ProcessManufacturers("manufacturers.csv");

            var query =
                from  car in cars
                group car by car.Manufacturer into carGroup
                select new
                {
                    Name    = carGroup.Key,
                    Max     = carGroup.Max(c => c.Combined),
                    Min     = carGroup.Min(c => c.Combined),
                    Average = carGroup.Average(c => c.Combined)
                }into result
                orderby result.Max descending
                select result;


            var query2 =
                cars.GroupBy(c => c.Manufacturer)
                    .Select(g =>
                    {
                        var result = g.Aggregate(new CarStatistics(),
                                            (acc, c) => acc.Accumulate(c),
                                            acc => acc.Compute());
                        return new
                        {
                            Name = g.Key,
                            Average = result.Average,
                            Min = result.Min,
                            Max = result.Max
                        };
                    })
                    .OrderByDescending(r => r.Max);

            foreach (var result in query2)
            {
                Console.WriteLine($"{result.Name}"    );
                Console.WriteLine($"\tMax       : {result.Max}"     );
                Console.WriteLine($"\tMin       : {result.Min}"     );
                Console.WriteLine($"\tAverage   : {result.Average}" );
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

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query =
                    File.ReadAllLines(path)
                        .Where(l => l.Length > 1)
                        .Select(l =>
                        {
                            var columns = l.Split(',');
                            return new Manufacturer
                            {
                                Name = columns[0],
                                Headquarters = columns[1],
                                Year = int.Parse(columns[2])
                            };
                        });
            return query.ToList();
        }

        private static object Queries(int queryPeeker)
        {
            // Queries earlier
            var cars = ProcessFile("fuel.csv");
            var mannufacturers = ProcessManufacturers("manufacturers.csv");

            var query2 =
                cars.Join(mannufacturers,
                            c => new { c.Manufacturer, c.Year },
                            m => new { Manufacturer = m.Name, m.Year },
                            (c, m) => new
                            {
                                m.Headquarters,
                                c.Name,
                                c.Combined
                            })
                    .OrderByDescending(c => c.Combined)
                    .ThenBy(c => c.Name);

            var query =
                from car in cars
                join mannufacturer in mannufacturers
                    on new { car.Manufacturer, car.Year }
                        equals
                        new { Manufacturer = mannufacturer.Name, mannufacturer.Year }
                orderby car.Combined descending, car.Name
                select new
                {
                    mannufacturer.Headquarters,
                    car.Name,
                    car.Combined
                };

            //var result = cars.SelectMany(c => c.Name)
            //                 .OrderBy(c => c);

            //foreach (var character in result)
            //{
            //    Console.WriteLine(character);
            //}
            if (queryPeeker == 1)
            {
                return query;
            }
            else if (queryPeeker == 2)
            {
                return query2;
            }
            else
            {
                return null;
            }
        }
    }

    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }
        public CarStatistics Accumulate(Car car)
        {
            Count += 1;
            Total += car.Combined;
            Max = Math.Max(Max, car.Combined);
            Min = Math.Min(Min, car.Combined);
            return this;
        }

        public CarStatistics Compute()
        {
            Average = Total / Count;
            return this;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
        public int Average { get; set; }
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
