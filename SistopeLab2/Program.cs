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
        public static Mutex mutZombie = new Mutex();
        public static Barrier barr = new Barrier(1, (bar) =>
                 {
                     mutex.WaitOne();
                     Console.Clear();
                     b.showBoard();
                     mutex.ReleaseMutex();
                 });

        public static Board b = new Board();
        public static Mutex mutex = new Mutex();

        public static List<int[]> zombiesToKill = new List<int[]>();
        public static int time = 0;
        

        public static void Main(string[] args)
        {
            Thread oBoard = new Thread(new ThreadStart(b.meterZombies));
            oBoard.Start();
            Program tim = new Program();
            Thread taiming = new Thread(new ThreadStart(tim.Timing));
            taiming.Start();

            while(true)
            {
                if(!taiming.IsAlive)
                {
                    Console.Clear();
                    mutex.WaitOne();
                    b.showBoard();
                    mutex.ReleaseMutex();
                    if (Board.zombiesNow > 0) 
                    { 
                        Console.WriteLine("Ganan los zombies al matar a todas las personas");
                        Console.WriteLine("Zombies restantes: " + Board.zombiesNow.ToString());
                    }
                    else 
                    { 
                        Console.WriteLine("Ganan las personas al matar a todos los zombies");
                        Console.WriteLine("Personas restantes: " + Board.personas.ToString());
                    }
                    break;
                }
            }
            Console.ReadLine();
            System.Environment.Exit(1);
        }

        public void Timing()
        {
            while (Board.personas > 0 && (Board.zombiesNow > 0 || Board.zombiesFaltantes > 0))
            {
                Thread.Sleep(1000);
                time++;
                barr.SignalAndWait();
            }
        }
    }
}
