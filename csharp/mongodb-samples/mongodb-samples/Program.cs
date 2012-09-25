using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mongodb_samples
{
    
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Child> Chidls { get; set; }
    }

    public class Child
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
