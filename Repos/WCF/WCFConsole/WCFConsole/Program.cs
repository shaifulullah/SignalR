using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference.Service1Client obj = new ServiceReference.Service1Client();
            var result = obj.AddData(2, 3);
            Console.WriteLine(result);
            Console.Read();
        }
    }
}
