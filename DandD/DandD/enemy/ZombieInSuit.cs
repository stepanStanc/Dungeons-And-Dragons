using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;
using MahApps.Metro.Controls;
using DandD.enemy;
using DandD.items;
using DandD.attack;
using DandD.interactions;

namespace DandD.enemy
{
    class ZombieInSuit : IEnemy
    {
        private int _HP = 250;

        public string Name { get { return "Zombie bussinesman"; } set { } }
        public int Strenght { get { return 15; } set { } }
        public int HP { get { return _HP; } set { _HP = value; } }
        public int maxHP { get { return 250; } }
        public int enWidth { get { return 70; } }

        public TimeSpan AttackSpeed { get { return new TimeSpan(0, 0, 0, 0, 160); } set { AttackSpeed = value; } }
        public TimeSpan RestTime { get { return new TimeSpan(0, 0, 0, 1); } set { RestTime = value; } }

        public ImageSource Img { get { return new BitmapImage(new Uri("pack://application:,,,/images/enemys/zombie_suit.gif")); } }
        public ImageSource ProjectileImg { get { return new BitmapImage(new Uri("pack://application:,,,/images/projectiles/brain.png")); } }
        public ImageSource Background { get { return new BitmapImage(new Uri("pack://application:,,,/images/textures/mud.jpg")); } }

        public TimeSpan MovementSpeed
        {
            get
            {
                TimeSpan tm;

                if (HP > 200)
                {
                    tm = new TimeSpan(0, 0, 0, 0, 20);
                }
                else if (HP > 80)
                {
                    tm = new TimeSpan(0, 0, 0, 0, 5);
                }
                else
                {
                    tm = new TimeSpan(0, 0, 0, 0, 2);
                }

                return tm;
            }

            set { MovementSpeed = value; }
        }

        public IAttackBehaviour AttackB
        {
            get
            {
                IAttackBehaviour atb;

                if (HP > 30)
                {
                    atb = new BasicAttack();
                }
                else
                {
                    atb = new RagingAttack();
                }

                return atb;
            }

            set { }
        }

        public int closeDmg() { return AttackB.attackPassive(Strenght); }
        public int rangedDmg() { return AttackB.attackProjectile(Strenght); }

    }
}
