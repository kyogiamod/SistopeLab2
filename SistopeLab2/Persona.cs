using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SistopeLab2
{
    class Persona : Entidad
    {
        public Weapon gun;
        bool infectado;
        int contadorTiempoVirus;

        public Persona(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool posible(int x, int y)
        {
            string posTab = Board.board[x, y].ToString();
            if (posTab.Equals("0") || posTab.Equals("G")) { return true; }
            return false;
        }
        public override string ToString()
        {
            if (infectado) { return "p"; }
            return "P";
        }

        public int[] correr(int[] posicion)
        {
            int[] pos = new int[2];
            if (this.x - posicion[0] == -1) 
            {
                if (!bordeSup(this.x)) { pos[0] = this.x - 1; }
            }
            else 
            {
                if (!bordeInf(this.x)) { pos[0] = this.x + 1; }
            }
            if(this.x - posicion[1] == -1)
            {
                if (!bordeIzq(this.y)) { pos[1] = this.y - 1; }
            }
            else
            {
                if (!bordeDer(this.y)) { pos[1] = this.y + 1; }
            }
            if (Board.board[pos[0], pos[1]].ToString().Equals("0") || Board.board[pos[0], pos[1]].ToString().Equals("G")) { return pos; }
            pos[0] = this.x;
            pos[1] = this.y;
            return pos;
        }

        public void move()
        {
            Random r = new Random();
            while (!_shouldStop)
            {
                if (this.contadorTiempoVirus >= 3) { while (true); }
                int i = 0;
                List<int[]> ValidPos2 = getPosiciones(this.x, this.y);   //crea una lista de todas las posiciones al rededor de donde esta.
                List<int[]> ValidPos = new List<int[]>();

                Program.mutex.WaitOne();
                //Deja solo las posiciones validas
                foreach (int[] posi in ValidPos2)
                {
                    if(posible(posi[0], posi[1])) { ValidPos.Add(posi); }
                }

                //Si puede moverse entonces se mueve
                if(ValidPos.Count() > 0)
                {
                    int[] pos = ValidPos[r.Next(0, ValidPos.Count())];
                    //Console.WriteLine("Persona: ({0},{1})", pos[0], pos[1]);
                    if (Board.board[pos[0], pos[1]].ToString().Equals("G"))
                    {   //Si el cuadro es una arma
                        this.gun = (Weapon)Board.board[pos[0], pos[1]]; //La toma
                    }
                    Board.board[pos[0], pos[1]] = this; //El espacio donde se movera la persona, es ahora la persona
                    Board.board[this.x, this.y] = new Piso();   //El espacio donde estaba ahora es un piso (vacio)
                    this.x = pos[0]; //cambia la posicion en x
                    this.y = pos[1]; //cambia la posicion en y
                }

                //Una vez que se mueve, se debe comprobar si en sus posiciones adyacentes hay un zombie
                List<int[]> adyacentes = getPosiciones(this.x, this.y);
                foreach(int[] posi in adyacentes)
                {
                    Object ob = Board.board[posi[0], posi[1]];
                    if(ob.ToString().Equals("Z"))
                    {   //Si el objeto es un zombie:
                        int caso = encuentro(this);
                        if (caso == 0) { this.infectado = true; }   //Zombie muerde humano
                        else if (caso == 1) 
                        {   //Humano mata a zombie
                            Program.mutZombie.WaitOne();
                            int[] posicion_zombie = new int[2];
                            posicion_zombie[0] = posi[0];
                            posicion_zombie[1] = posi[1];
                            Program.zombiesToKill.Add(posicion_zombie);
                            
                            Program.mutZombie.ReleaseMutex();
                            //this.gun.bullets--;
                        }
                        else if (caso == 2) 
                        {   //Humano corre
                            int[] toRun = correr(posi);
                            if(toRun[0] != this.x || toRun[1] != this.y)
                            {
                                Board.board[toRun[0], toRun[1]] = this;
                                Board.board[this.x, this.y] = new Piso();
                            }
                            this.x = toRun[0];
                            this.y = toRun[1];
                        }

                    }
                }
                Program.mutex.ReleaseMutex();
                //Ahora que ya hizo su jugada, debe ponerse en la barrera.
                if (this.infectado) 
                { 
                    this.contadorTiempoVirus++;
                    if (this.contadorTiempoVirus == 3) 
                    {
                        Program.mutex.WaitOne();
                        Board.createZombie(this.x, this.y);
                        Program.mutex.ReleaseMutex();
                        _shouldStop = true;
                    }
                    else
                    {
                        Program.barr.SignalAndWait();
                    }
                }
                
                else
                {
                    Program.barr.SignalAndWait();
                }
            }
            //El virus T lo convirtió en zombie, se muere el humano y se desinscribe de la barrera
            Board.personas--;
            Program.barr.RemoveParticipant();
        }
    }
}
