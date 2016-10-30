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
    }
}
