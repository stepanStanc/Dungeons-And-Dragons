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
    class Blob : IEnemy
    {
        private int _HP = 800;

        public string Name { get { return "Blob"; } set { } }
        public int Strenght { get { return 40; } set { } }
        public int HP { get { return _HP; } set { _HP = value; } }
        public int maxHP { get { return 800; } }
        public int enWidth { get { return 150; } }

        public TimeSpan AttackSpeed { get { return new TimeSpan(0, 0, 0, 0, 20); } set { AttackSpeed = value; } }
        public TimeSpan RestTime { get { return new TimeSpan(0, 0, 0, 1); } set { RestTime = value; } }

        public ImageSource Img { get { return new BitmapImage(new Uri("pack://application:,,,/images/enemys/blob.gif")); } }
        public ImageSource ProjectileImg { get { return new BitmapImage(new Uri("pack://application:,,,/images/projectiles/slimeBall.png")); } }
        public ImageSource Background { get { return new BitmapImage(new Uri("pack://application:,,,/images/textures/swamp.jpg")); } }

        public TimeSpan MovementSpeed
        {
            get
            {
                return new TimeSpan(0, 0, 0, 0, 70);
            }

            set { MovementSpeed = value; }
        }

        public IAttackBehaviour AttackB
        {
            get
            {
               
                return new BasicAttack();
            }

            set { }
        }

        public int closeDmg() { return AttackB.attackPassive(Strenght); }
        public int rangedDmg() { return AttackB.attackProjectile(Strenght); }

    }
}
