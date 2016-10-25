using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistopeLab2
{
    class Weapon
    {
        public int bullets;

        public Weapon()
        {
            this.bullets = 0;
        }
        public Weapon(int bullets)
        {
            this.bullets = bullets;
        }

        public override string ToString()
        {
            return "G";
        }
    }
}
