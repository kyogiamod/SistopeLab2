using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistopeLab2
{
    class Zombie : Entidad
    {

        public Zombie(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void move()
        {
            Random r = new Random();
            //Program.mutex.WaitOne();
            while (!_shouldStop)
            {
                Program.mutZombie.WaitOne();
                int i = 0;
                if(Program.zombiesToKill.Count > 0)
                {
                    foreach (int[] pos in Program.zombiesToKill)
                    {
                        if (pos[0] == this.x && pos[1] == this.y)
                        {
                            _shouldStop = true;
                            Program.zombiesToKill.RemoveAt(i);
                        }
                        i++;
                    }
                }
                Program.mutZombie.ReleaseMutex();
                if (_shouldStop) { break; }
                List<int[]> ValidPos2 = getPosiciones(this.x, this.y);   //crea una lista de todas las posiciones al rededor de donde esta.
                List<int[]> ValidPos = new List<int[]>();

                Program.mutex.WaitOne();
                //Deja solo las posiciones validas
                foreach (int[] posi in ValidPos2)
                {
                    if (posible(posi[0], posi[1])) { ValidPos.Add(posi); }
                }

                //Si puede moverse entonces se mueve
                if (ValidPos.Count() > 0)
                {
                    int[] pos = ValidPos[r.Next(0, ValidPos.Count())];
                    Console.WriteLine("Zombie (" + pos[0].ToString() + "," + pos[1].ToString() + ").");
                    Board.board[pos[0], pos[1]] = this; //El espacio donde se movera la persona, es ahora la persona
                    Board.board[this.x, this.y] = new Piso();   //El espacio donde estaba ahora es un piso (vacio)
                    this.x = pos[0]; //cambia la posicion en x
                    this.y = pos[1]; //cambia la posicion en y
                }
                Program.mutex.ReleaseMutex();
                //Ahora que ya hizo su jugada, debe ponerse en la barrera.
                Program.barr.SignalAndWait();
            }
            Program.barr.RemoveParticipant();
        }
        public override string ToString()
        {
            return "Z";
        }
    }
}
