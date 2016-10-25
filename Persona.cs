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
            string posTab = Program.b.board[x, y].ToString();
            if (posTab.Equals("0") || posTab.Equals("G")) { return true; }
            return false;
        }
        public override string ToString()
        {
            return "P";
        }

        public void move()
        {
            Random r = new Random();
            while (true)
            {
                int i = 0;
                List<int[]> ValidPos2 = getPosiciones(this.x, this.y);   //crea una lista de todas las posiciones al rededor de donde esta.
                List<int[]> ValidPos = new List<int[]>();

                //Deja solo las posiciones validas
                foreach (int[] posi in ValidPos2)
                {
                    if(posible(posi[0], posi[1])) { ValidPos.Add(posi); }
                }

                //Si puede moverse entonces se mueve
                if(ValidPos.Count() > 0)
                {
                    int[] pos = ValidPos[r.Next(0, ValidPos.Count())];
                    if (Program.b.board[pos[0], pos[1]].ToString().Equals("G"))
                    {   //Si el cuadro es una arma
                        this.gun = (Weapon)Program.b.board[pos[0], pos[1]]; //La toma
                    }
                    Program.b.board[pos[0], pos[1]] = this; //El espacio donde se movera la persona, es ahora la persona
                    Program.b.board[this.x, this.y] = new Piso();   //El espacio donde estaba ahora es un piso (vacio)
                    this.x = pos[0]; //cambia la posicion en x
                    this.y = pos[1]; //cambia la posicion en y
                }

                //Ahora que ya hizo su jugada, debe ponerse en la barrera.
                Program.barr.SignalAndWait();
            }
        }
    }
}
