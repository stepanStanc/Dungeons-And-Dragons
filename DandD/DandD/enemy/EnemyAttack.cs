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
    /// <summary>
    /// útoky nepřítele
    /// </summary>
    class EnemyAttack
    {
        private basicInteractions interact = new basicInteractions();
        private MainWindow c;

        private int loop = 1;
        private int enFiringL = 0;
        private int enFiringT = 0;
        private bool firedFromRight;
        private Image enemyProjectile;
        private Canvas fight_canvas;
        private Image enemyControl;
        private IEnemy enemy;

        public EnemyAttack(IEnemy en)
        {
            c = interact.getContext();

            enemyProjectile = c.enemyProjectile;
            fight_canvas = c.fight_canvas;
            enemyControl = c.enemyControl;

            enemy = en;
        }

        public void fire(bool goesRight) //výstřel
        {
            if (c.enFireUnblocked)
            {
                //spustí omezení střel
                c.enFireDelay.Start();
                endpProjectileMovement();

                enemyProjectile.Source = c.enemy.ProjectileImg;
                enemyProjectile.Width = 50;
                enemyProjectile.RenderTransformOrigin = new Point(0.5, 0.5);

                firedFromRight = goesRight;

                enFiringL = Convert.ToInt32(Canvas.GetLeft(enemyControl));
                enFiringT = Convert.ToInt32(Canvas.GetTop(enemyControl));

                if (!goesRight)
                {
                    Canvas.SetLeft(enemyProjectile, enFiringL + 50 + loop * 8);
                }
                else
                {
                    Canvas.SetLeft(enemyProjectile, enFiringL - loop * 8);
                }

                Canvas.SetTop(enemyProjectile, enFiringT + 30);

                fight_canvas.Children.Add(enemyProjectile);
                if (goesRight)
                {
                    interact.faceLeft(enemyProjectile);
                }
                else
                {
                    interact.faceRight(enemyProjectile);
                }

                c.enemyProjectileMovement.Start();
                c.enFireUnblocked = false;
            }
        }

        public void projectileMovementFc() // posouvá střelu
        {
            loop++; // na začátku aby nemělo 0 hodnotu - všechny loopy

            if (!firedFromRight)
            {
                Canvas.SetLeft(enemyProjectile, enFiringL + 50 + loop * 9);
            }
            else
            {
                Canvas.SetLeft(enemyProjectile, enFiringL - loop * 9);
            }

            if (Convert.ToInt32(Math.Floor(fight_canvas.ActualWidth)) < (Convert.ToInt32(Canvas.GetLeft(enemyProjectile)) + 20) || (Convert.ToInt32(Canvas.GetLeft(enemyProjectile)) + 50 < 0))
            {
                endpProjectileMovement();
            }

            if (interact.overlap(c.enPrRect, c.plRect))
            {
                int dmg = 1;

                if (c.p.shieldActive)
                {
                    dmg = Convert.ToInt32(c.enemy.rangedDmg() * (100 - c.p.Shield.armor)/ 100);
                }
                else
                {
                    dmg = c.enemy.rangedDmg();
                }
                c.hp.Value -= dmg; // projectile damage
                c.p.HP -= dmg;
                interact.Hit(dmg, c.playerControl);
                endpProjectileMovement();
                interact.fadeInOut(c.plTakenDmg);

                if (c.p.HP <= 0)
                {
                    interact.FightEnded();
                }
            }

            c.enPrRect = interact.ConvertToRect(enemyProjectile);
        }

        public void endpProjectileMovement()
        {
            loop = 0;
            fight_canvas.Children.Remove(enemyProjectile);
            c.enemyProjectileMovement.Stop();
        }

    }
}
