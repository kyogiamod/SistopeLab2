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
            List<int[]> ValidPos = getPosiciones(this.x, this.y);
            int total = ValidPos.Count();
            Random r = new Random();
            bool posiblePoner;
            do
            {
                int[] pos = ValidPos[r.Next(0, total)];
                posiblePoner = posible(pos[0], pos[1]);
                if (posiblePoner)
                {
                    Program.b.board[pos[0], pos[1]] = this;
                    Program.b.board[this.x, this.y] = new Piso();
                    this.x = x;
                    this.y = y;
                }
            } while (!posiblePoner);
        }
        public override string ToString()
        {
            return "Z";
        }
    }
}
