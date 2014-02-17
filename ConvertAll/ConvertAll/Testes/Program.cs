using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Conversion;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //number to string output: 1
            var numberToString = 1.ConvertTo<string>();
            Console.WriteLine(numberToString);

            //empty string to DateTime output: 01/01/0001 00:00:00
            var emptyStringToDateTime = "".ConvertTo<DateTime>();
            Console.WriteLine(emptyStringToDateTime);

            //string to DateTime output: 12/12/2012 00:00:00
            var stringToDateTime = "12/12/2012".ConvertTo<DateTime>();
            Console.WriteLine(stringToDateTime);

            //list to stack output: A stack with 3 positions
            var listToStack = 
                new List<string>(
                    new string[] { "1", "2", "3" })
                    .ConvertTo<Stack<string>>();
            Console.WriteLine(listToStack);

            //null to int output = 0
            var nullToSomeType = ((object)null).ConvertTo<int>();
            Console.WriteLine(nullToSomeType);

            //null to nullable int output = null
            var nullToNullable = ((object)null).ConvertTo<int?>();
            Console.WriteLine(nullToNullable);

            Console.ReadLine();

        }
    }
}
