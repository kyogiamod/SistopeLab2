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
        public object[,] board;

        public Board()
        {
            System.String path = Directory.GetCurrentDirectory();   //ruta actual
            path = path + "\\in.txt";   //direccion del archivo 

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string[] datos = file.ReadLine().Split(' ');

            this.ancho = Int16.Parse(datos[0]);
            this.alto = Int16.Parse(datos[1]);
            this.board = new object[this.alto, this.ancho];
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
                    if (c == 'X') { this.board[i, j] = new Wall(); }
                    else if (c == 'E') { this.board[i, j] = new Entrada(); }
                    else if (c == '0') { this.board[i, j] = new Piso(); }
                    else if(c == 'Z')
                    {
                        Zombie zomb = new Zombie(i, j);
                        this.board[i, j] = zomb;
                        Thread oZombie = new Thread(new ThreadStart(zomb.move));
                        Threads.Add(oZombie);
                    }
                    else if (c == 'G') { this.board[i, j] = new Weapon(); }
                    else if(c == 'P')
                    {
                        Persona p = new Persona(i, j);
                        this.board[i, j] = p;
                        Thread op = new Thread(new ThreadStart(p.move));
                        Threads.Add(op);
                    }
                    j++;
                }
                i++;
            }
            this.showBoard();
            foreach(Thread t in Threads)
            {
                t.Start();
            }

        }

        public void showBoard()
        {
            int i = 0, j = 0;
            while (i < this.alto)
            {
                j = 0;
                while (j < this.ancho)
                {
                    object o = this.board[i, j];
                    string toPrint = o.ToString();
                    if(toPrint != "0")
                    {
                        Console.Write(this.board[i, j].ToString());
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
