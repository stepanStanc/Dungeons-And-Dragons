using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DandD.attack;

namespace DandD.enemy
{
    /// <summary>
    /// interface nepřítele s jeho proměnitelnými staty a iconami, rychlostmi
    /// </summary>
    public interface IEnemy
    {
        string Name { get; set; }

        int Strenght { get; set; }
        int HP { get; set; }
        int maxHP { get; }    
        int enWidth { get; }

        IAttackBehaviour AttackB { get; set; }

        TimeSpan AttackSpeed { get; set; }
        TimeSpan MovementSpeed { get; set; }
        TimeSpan RestTime { get; set; }

        ImageSource Img { get; }
        ImageSource Background { get; }
        ImageSource ProjectileImg { get; }

        int closeDmg();
        int rangedDmg();

        /*
         ZMĚNY V CHOVÁNÍ NEPŘÍTELE PŘI SOUBOJI
         - zrychlení
         - delší odpočinky
         - větší meší/síla
         - nepravidelná rychlost
         */

    }
}
