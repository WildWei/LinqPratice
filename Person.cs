using System;
using System.Collections.Generic;
using System.Linq;

namespace TestModel
{
    public class Person
    {
        private static Person _empty = new Person();
        public static Person Empty => _empty;

        /// <summary>
        /// 男:M , 女:F
        /// </summary>
        public char gender { get; set; }

        public int Age { get; set; }

        public string Name { get; set; }

        public int Salery { get; set; }
    }
}
