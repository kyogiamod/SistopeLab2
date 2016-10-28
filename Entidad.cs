using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistopeLab2
{
    class Entidad
    {
        public int x;
        public int y;
        public object oLock = new Object();
        public volatile bool _shouldStop = false;

        public bool bordeSup(int x)
        {
            if (x == 0) { return true; }
            return false;
        }
        public bool bordeInf(int x)
        {
            if (x == Program.b.alto - 1) { return true; }
            return false;
        }
        public bool bordeIzq(int y)
        {
            if (y == 0) { return true; }
            return false;
        }
        public bool bordeDer(int y)
        {
            if (y == Program.b.ancho - 1) { return true; }
            return false;
        }
        public virtual bool posible(int x, int y)
        {
            if (Board.board[x, y].ToString().Equals("0")) { return true; }
            return false;
        }

        public int lanzarDado(int inicial, int final)
        {
            Random r = new Random();
            return r.Next(inicial, final);
        }

        public int encuentro(Persona p)
        {   //0 si humano pierde. 1 si humano mata al zombie o 2 si humano corre
            if (lanzarDado(1, 11) >= 4) { return 0; }
            else
            {
                if(p.gun != null)
                {
                    if (p.gun.bullets > 0) { return 1; }
                }
                return 2;
            }
        }
        public List<int[]> getPosiciones(int x, int y)
        {
            List<int[]> pos = new List<int[]>();
            
            int[] norte = { x - 1, y };
            int[] noreste = { x - 1, y + 1 };
            int[] este = { x, y + 1 };
            int[] sureste = { x + 1, y + 1 };
            int[] sur = { x + 1, y };
            int[] suroeste = { x + 1, y - 1 };
            int[] oeste = { x, y - 1 };
            int[] noroeste = { x - 1, y - 1 };

            pos.Add(norte);
            pos.Add(noreste);
            pos.Add(este);
            pos.Add(sureste);
            pos.Add(sur);
            pos.Add(suroeste);
            pos.Add(oeste);
            pos.Add(noroeste);

            for(int i = 0; i < pos.Count; i++)
            {
                int equis = pos[i][0];
                int ye = pos[i][1];

                if (equis < 0) { pos.RemoveAt(i); }
                else if (equis >= Program.b.alto) { pos.RemoveAt(i); i--; }
                else if (ye < 0) { pos.RemoveAt(i); i--; }
                else if (ye >= Program.b.ancho) { pos.RemoveAt(i); i--; }
            }
            return pos;
        }
    }
}
