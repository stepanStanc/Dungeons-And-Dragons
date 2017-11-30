using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;
using MahApps.Metro.Controls;
using System.Windows.Threading;
using DandD.interactions;
using DandD.enemy;
using DandD.tabBehaviour;
using DandD.items;

namespace DandD
{
    class playerPrFlight
    {
        private basicInteractions interact = new basicInteractions();
        private DispatcherTimer flightTimer = new DispatcherTimer();
        private MainWindow c;
        private bool firedFromRight;
        private int firingL;
        private int loop = 1;
        private Rect plPrRect = new Rect();
        private List<Image> plProjectiles;
        private int i = 0;

        public playerPrFlight(bool firFR,int current,int frL)
        {
            c = interact.getContext();
            i = current;

            firingL = frL;

            firedFromRight = firFR;
            plProjectiles = c.plProjectiles;

            Random r = new Random();
            int speed = r.Next(2,15 );

            flightTimer.Tick += new EventHandler(projectileMovementFc);
            flightTimer.Interval = new TimeSpan(0, 0, 0, 0, speed);
        }

        public void Start()
        {
            flightTimer.Start();
        }

        private void projectileMovementFc(object sender, EventArgs e) // funkce posouvající projektil a detekojícístřet
        {
            //MessageBoxResult result = MessageBox.Show(loop.ToString());
            plPrRect = interact.ConvertToRect(plProjectiles[i]);

            var enPrRect = c.enPrRect;
            var enRect = c.enRect;

            if (!firedFromRight)
            {
                Canvas.SetLeft(plProjectiles[i], firingL + 50 + loop * 8);
            }
            else
            {
                Canvas.SetLeft(plProjectiles[i], firingL - loop * 8);
            }

            //pokud dosáhl konce canvasu
            if (Convert.ToInt32(Math.Floor(c.fight_canvas.ActualWidth)) < (plPrRect.Left + 20) || (plPrRect.Left + 50 < 0))
            {
                endpProjectileMovement();
            }

            //pokud trefil nepřítele
            if (interact.overlap(plPrRect, enRect))
            {
                int dmg = c.p.Strenght + c.p.RangedWeapon.strenght;
                c.en_hp.Value -= dmg; // projectile damage
                c.enemy.HP -= dmg;
                interact.Hit(dmg, c.enemyControl);

                endpProjectileMovement();
                interact.fadeInOut(c.enTakenDmg);

                if (c.enemy.HP <= 0)
                {
                    c.p.LVL++;
                    c.p.won = true;
                    interact.FightEnded();
                }
            }

            //pokud se setkal s projektilem nepřítele = může zničit projektil nepřítele
            if (c.fight_canvas.Children.Contains(c.enemyProjectile)) { 
                if (interact.overlap(plPrRect, enPrRect))
                {
                    endpProjectileMovement();

                    EnemyAttack ea = new EnemyAttack(new Zombie());
                    ea.endpProjectileMovement();
                }
            }

            loop++; // na začátku aby nemělo 0 hodnotu - v
            plPrRect = interact.ConvertToRect(plProjectiles[i]);
        }

        public void endpProjectileMovement()
        {
            loop = 0;
            c.fight_canvas.Children.Remove(plProjectiles[i]);
            flightTimer.Stop();
        }
    }
}
