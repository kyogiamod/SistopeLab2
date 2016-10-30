using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SistopeLab2
{
    class Board
    {
        public int ancho;
        public int alto;
        public static int zombiesNow = 0;
        public static int totalZombies;
        public static int personas = 0;
        public static int zombiesFaltantes;
        public static object[,] board;
        public static List<int[]> listaEntrada = new List<int[]>();
        public static Mutex mutBoard = new Mutex();

        public Board()
        {
            System.String path = Directory.GetCurrentDirectory();   //ruta actual
            path = path + "\\in.txt";   //direccion del archivo 

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string[] datos = file.ReadLine().Split(' ');

            this.ancho = Int16.Parse(datos[0]);
            this.alto = Int16.Parse(datos[1]);
            board = new object[this.alto, this.ancho];
            int ammo = Int16.Parse(datos[4]);
            Board.totalZombies = Int16.Parse(datos[2]);
            zombiesFaltantes = totalZombies;


            List<string> lines = new List<string>();

            string line;
            while((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }
            file.Close();
            //Leido el archivo, ahora se procede a poner los datos en el tablero.

            int i = 0; 
            int j = 0;
            List<Thread> Threads = new List<Thread>();
            foreach(string s in lines)
            {
                j = 0;
                foreach(char c in s)
                {
                    if (c == 'X') { board[i, j] = new Wall(); }
                    else if (c == 'E') 
                    {
                        Entrada en = new Entrada(i, j);
                        board[i, j] = en;
                        int[] p = new int[2];
                        p[0] = i;
                        p[1] = j;
                        listaEntrada.Add(p);
                    }
                    else if (c == '0') { board[i, j] = new Piso(); }
                    //else if (c == 'Z') { Program.barr.AddParticipant(); }
                    else if (c == 'G') { board[i, j] = new Weapon(ammo); }
                    else if(c == 'P')
                    {
                        Persona p = new Persona(i, j);
                        board[i, j] = p;
                        Thread op = new Thread(new ThreadStart(p.move));
                        Threads.Add(op);
                        Board.personas++;
                        Program.barr.AddParticipant();
                    }
                    j++;
                }
                i++;
            }
            showBoard();
            foreach(Thread t in Threads)
            {
                t.Start();
            }
        }

        public void meterZombies()
        {
            while (Board.zombiesFaltantes > 0)
            {
                foreach (int[] pos in listaEntrada)
                {
                    if (Board.zombiesFaltantes > 0)
                    {

                        Zombie z = new Zombie(pos[0], pos[1]);
                        Thread oZombie = new Thread(new ThreadStart(z.move));
                        Program.barr.AddParticipant();
                        oZombie.Start();
                        mutBoard.WaitOne();
                        Board.zombiesNow++;
                        Board.zombiesFaltantes--;
                        mutBoard.ReleaseMutex();
                    }
                }
                Thread.Sleep(3000);
            }
        }

        public static void createZombie(int x, int y)
        {
            Zombie oz = new Zombie(x, y, 1);
            board[x, y] = oz;
            
            Thread oZombie = new Thread(new ThreadStart(oz.move));
            oZombie.Start();
            Program.barr.AddParticipant();
            mutBoard.WaitOne();
            Board.zombiesNow++;
            mutBoard.ReleaseMutex();
        }
        
        public void showBoard()
        {
            string letra;
            int i = 0, j = 0;
            while (i < this.alto)
            {
                j = 0;
                while (j < this.ancho)
                {
                    object o = board[i, j];
                    string toPrint = o.ToString();
                    if(!toPrint.Equals("0"))
                    {
                        letra = board[i, j].ToString();
                        if(letra.Equals("P") || letra.Equals("p"))
                        {
                            if(letra.Equals("P"))   { Console.ForegroundColor = ConsoleColor.Red; }
                            else {Console.ForegroundColor = ConsoleColor.Magenta;}
                            Console.Write(letra);

                            Console.ResetColor();
                        }
                        else if (letra.Equals("Z"))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(letra);

                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write(letra);
                        }
                    }
                    else { Console.Write(" "); }
                    j++;
                }
                Console.WriteLine();
                i++;
            }
            Console.WriteLine("Numero de zombies: {0}", Board.zombiesNow);
            Console.WriteLine("Numero de personas: {0}", Board.personas);
            Console.WriteLine("Tiempo transcurrido: {0}[segundos]", Program.time);
        }
    }
}
