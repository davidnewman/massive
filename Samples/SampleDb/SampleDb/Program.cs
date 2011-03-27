using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Massive;
using Massive.Ext;

namespace SampleDb
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new ContactsDb("SampleDb");

            foreach (var c in db.Contact.All())
            {
                Console.WriteLine("{0} {1} lives at:", c.FirstName, c.LastName);

                IEnumerable<dynamic> addresses = c.Addresses();

                if (addresses.Any())
                {
                    foreach (var a in addresses)
                    {
                        Console.WriteLine("\t{0}, {1}, {2} {3}", a.Street, a.City, a.State, a.Zip);
                    }
                }
                else
                {
                    Console.WriteLine("\tNo address on file");
                }

            }

            Console.ReadLine();
        }
    }
}
