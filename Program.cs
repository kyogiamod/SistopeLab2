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
        public static Barrier barr = new Barrier(23, (bar) =>
                 {
                     //Thread.Sleep(1000);
                     if (barr.CurrentPhaseNumber > faseAnterior)
                     {
                         Console.Clear();
                         b.showBoard();
                         faseAnterior++;
                         pasar = true;
                     }
                 });
        public static Board b = new Board();

        public static int faseAnterior = 0;
        public static bool pasar = false;
    static void Main(string[] args)
        {
            while (true)
            {
            }
        }
    }
}
