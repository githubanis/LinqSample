using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Introduction
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\windows";
            ShowLargeFileWithoutLinq(path);
            Console.WriteLine("***");
            ShowLargeFileWithLinq(path);
        }

        private static void ShowLargeFileWithLinq(string path)
        {
            var query = from file in new DirectoryInfo(path).GetFiles()
                        orderby file.Length descending
                        select file; 

            foreach (var file in query.Take(5))
            {
                Console.WriteLine($"{file.Name,-60} : {file.Length,10:N0}");
            }
        }

        private static void ShowLargeFileWithoutLinq(string path)
        {
            var directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles();
            Array.Sort(files, new FileInfoCompare());
            for (int i = 0; i < 5; i++)
            {
                FileInfo file = files[i];
                Console.WriteLine($"{file.Name, -60} : {file.Length, 10:N0}");
            }
        }
    }

    public class FileInfoCompare : IComparer<FileInfo>
    {
        public int Compare([AllowNull] FileInfo x, [AllowNull] FileInfo y)
        {
            return y.Length.CompareTo(x.Length);
        }
    }
}
