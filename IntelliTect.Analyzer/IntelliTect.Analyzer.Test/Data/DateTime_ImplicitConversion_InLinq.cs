using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class Pair
    {
        public DateTimeOffset DateTimeOffset { get; init; }
        public DateTime DateTime { get; init; }

        public Pair(DateTimeOffset dateTimeOffset, DateTime dateTime)
        {
            DateTimeOffset = dateTimeOffset;
            DateTime = dateTime;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Pair> list = new(){ new(DateTimeOffset.Now, DateTime.Now) };
            _ = list.Where(pair => pair.DateTimeOffset < pair.DateTime);
        }
    }
}
