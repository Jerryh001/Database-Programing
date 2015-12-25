using System;
using System.Linq;

namespace LinqTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (NorthwindEntities NE = new NorthwindEntities())
            {
                var query =
                    from item in NE.Alphabetical_list_of_products
                    select new
                    {
                        a = item.CategoryName,
                        b = item.ProductName,
                        c = item.QuantityPerUnit
                    };
                foreach(var i in query)
                {
                    Console.WriteLine("a={0}, b={1},c={2}", i.a, i.b, i.c);
                }
            }
            Console.ReadLine();
        }
    }
}
