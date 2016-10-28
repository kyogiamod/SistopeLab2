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
        public int zombiesNow;
        public int totalZombies;
        public int personas;
        public int entradas;
        public static object[,] board;

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
                    else if (c == 'E') { board[i, j] = new Entrada(); }
                    else if (c == '0') { board[i, j] = new Piso(); }
                    else if(c == 'Z')
                    {
                        Zombie zomb = new Zombie(i, j);
                        board[i, j] = zomb;
                        Thread oZombie = new Thread(new ThreadStart(zomb.move));
                        Threads.Add(oZombie);
                        Program.barr.AddParticipant();
                    }
                    else if (c == 'G') { board[i, j] = new Weapon(); }
                    else if(c == 'P')
                    {
                        Persona p = new Persona(i, j);
                        board[i, j] = p;
                        Thread op = new Thread(new ThreadStart(p.move));
                        Threads.Add(op);
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

        public static void createZombie(int x, int y)
        {
            Zombie oz = new Zombie(x, y);
            board[x, y] = oz;
            Thread oZombie = new Thread(new ThreadStart(oz.move));
            oZombie.Start();
            Program.barr.AddParticipant();
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
                    if(toPrint != "0")
                    {
                        //Console.Write(board[i, j].ToString());
                        letra = board[i, j].ToString();

                        if(letra.Equals("P"))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
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
        }
    }
}
