using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DandD.attack;

namespace DandD.enemy
{
    class BasicAttack : IAttackBehaviour
    {
        public int attackPassive ( int strenght)
        {
            return strenght;
        }

        public int attackProjectile( int strenght)
        {
            return strenght + Convert.ToInt32(Math.Ceiling(strenght * 0.4));
        }
    }
}
