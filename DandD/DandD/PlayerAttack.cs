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
using DandD;


namespace DandD
{
    /// <summary>
    /// útoky hráče
    /// </summary>
    class PlayerAttack
    {
        private basicInteractions interact = new basicInteractions();

        private Rectangle playerWeapon = new Rectangle();
        private bool firedFromRight = false;
        private Image playerControl;
        private Canvas fight_canvas;
        private int firingL;
        private int firingT;
        private Rect plPrRect = new Rect(0, 0, 0, 0);
        private List<Image> plProjectiles;
        private MainWindow c;
        private Player pl;
        private int i = 0;

        public PlayerAttack(Player p)
        {
            c = interact.getContext();
            playerWeapon = c.playerWeapon;
            playerControl = c.playerControl;
            fight_canvas = c.fight_canvas;     
            plProjectiles = c.plProjectiles;
            pl = p;
        }


        public void close() //útok na blízko
        {
            if (interact.overlap(c.wpRect, c.enRect))
            {
                // zde se bude využívat třídy
                int dmg = pl.Strenght + pl.Weapon.strenght;
                c.en_hp.Value -= dmg;
                c.enemy.HP -= dmg;
                interact.Hit(dmg, c.enemyControl);
                if (c.enemy.HP <= 0)
                {                   
                    c.p.LVL++;
                    c.p.won = true;
                    interact.FightEnded();
                }

                interact.fadeInOut(c.enTakenDmg);

            }
            
            //může zničit projektil nepřítele
            if (interact.overlap(c.wpRect, c.enPrRect))
            {
                EnemyAttack ea = new EnemyAttack(new Zombie());
                ea.endpProjectileMovement();
            }
            
        }

        /// <summary>
        /// Vytvoří a spustí let projektilu
        /// </summary>
        public void fire(bool PgoesRight)
        {
            //endpProjectileMovement();
            plProjectiles.Add(new Image());
            firedFromRight = PgoesRight; //pozice vystřelení

            plProjectiles[i].Source = new BitmapImage(new Uri("pack://application:,,,/images/projectiles/arrow.png")); //obrázek
            plProjectiles[i].Width = 50;
            plProjectiles[i].RenderTransformOrigin = new Point(0.5, 0.5);


            firingL = Convert.ToInt32(c.plRect.Left);
            firingT = Convert.ToInt32(c.plRect.Top);

            //nstavení prvůtního  výstřelu
            if (!firedFromRight)
            {
                Canvas.SetLeft(plProjectiles[i], firingL + 50 + 1 * 8);
            }
            else
            {
                Canvas.SetLeft(plProjectiles[i], firingL + 1 * 8);
            }
            Canvas.SetTop(plProjectiles[i], firingT + 30);

            //vložení střely do canvasu
            fight_canvas.Children.Add(plProjectiles[i]);

            //nastavení spěru projektilu (graficky)
            if (firedFromRight)
            {
                interact.faceLeft(plProjectiles[i]);
            }
            else
            {
                interact.faceRight(plProjectiles[i]);
            }

            //spustí let
            playerPrFlight start = new playerPrFlight(firedFromRight,i,firingL);
            start.Start();
            i++;
        }             

    }
}
