using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistopeLab2
{
    class Entrada
    {
        public Zombie zomb;
        public int x;
        public int y;

        public Entrada(int i, int j)
        {
            this.x = i;
            this.y = j;
        }
        public override string ToString()
        {
            if (zomb != null) { return "Z"; }
            return "E";
        }

        public void comprobarEntrada()
        {
            while(zomb != null)
            {
                if (this.x != zomb.x || this.y != zomb.y) { zomb = null; }
            }
        }
        public bool entrando()
        {
            if (zomb == null) { return false; }
            return true;
        }
    }
}
