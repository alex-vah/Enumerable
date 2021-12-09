using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enumerable
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string a = "ABCD";
            string b = "B";
            var i = a._Union(b);
            var d = a._First(c => a == "ABCD");
            var k = a._FirstOrDefault(c => a == c.ToString());
            var z = a._Take('a');
            var s = a._TakeWhile(c => a == "S");
            var p =a._SkipWhile(c => a == "A");
            var q = a._Skip('a');
            var v = a._Distinct();

            
        }
    }
}
