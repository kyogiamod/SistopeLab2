using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace SistopeLab2
{
    class Program
    {
        public static Barrier barr = new Barrier(1, (bar) =>
                 {
                     Console.Clear();
                     b.showBoard();
                 });

        public static Board b = new Board();
        public static Mutex mutex = new Mutex();

        public static List<int[]> zombiesToKill = new List<int[]>();
        public static Mutex mutZombie = new Mutex();

    static void Main(string[] args)
        {
            while(true)
            {
                barr.SignalAndWait();
                Thread.Sleep(1000);
            }
        }
    }
}
