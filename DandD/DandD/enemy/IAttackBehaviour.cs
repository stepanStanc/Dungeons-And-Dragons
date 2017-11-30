using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DandD.attack
{
    //interface útoku - zpracování dmg projektilu a contaktní dmg
    public interface IAttackBehaviour
    {
        int attackPassive( int strenght); // passivní dmg
        int attackProjectile( int strenght); // dmg projektilu
    }
}
