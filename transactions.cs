using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Practice_3
{
    internal class transactions
    {
        public string Sender {  get; set; }
        public string Receiver { get; set; }
        public double Sum { get; set; }
        public DateTime Time { get; set; }

        public static transactions ParseFile(string line)
        {
            string[] parts = line.Split(';');
            return new transactions()
            {
                Sender = parts[0],
                Receiver = parts[1],
                Sum = double.Parse(parts[2]),
                Time = DateTime.Parse(parts[3])
            };
        }

    }
}
