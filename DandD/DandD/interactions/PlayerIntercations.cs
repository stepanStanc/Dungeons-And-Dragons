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
using System.Windows.Threading;

namespace DandD.interactions
{
    class PlayerIntercations
    {
        
        private Player pl = new Player();
        private basicInteractions interact = new basicInteractions();
        private DispatcherTimer followMouse = new DispatcherTimer();
        private ImageBrush weaponBrush = new ImageBrush();
        private Rectangle playerWeapon;
        private double lastPx = 0;
        private bool PgoesRight;
        private Image playerControl;
        private MainWindow c;
        private double myX;
        private double myY;
        private bool moves = false;
        private double speed = 8;

        private bool meele = true;
        private bool ranged = false;

        public PlayerAttack attack;

        public PlayerIntercations()
        {
            c = interact.getContext();
            pl = c.p;
            weaponBrush.ViewboxUnits = BrushMappingMode.Absolute;
            weaponBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/items/items2.png"));

            playerWeapon = c.playerWeapon;
            playerControl = c.playerControl;
            PgoesRight = c.PgoesRight;
            attack = new PlayerAttack(pl);
            playerBasicPositions(500, 200);

            followMouse.Tick += new EventHandler(followingMouse);
            followMouse.Interval = new TimeSpan(0, 0, 0, 0, 6);

            speed = pl.speed;
        }
        /*
        public void pickAttack(bool l,bool r,bool m) // vybere útok podle tlačítka
        {
            var endurance = c.endurance;
            var mana = c.mana;
            pl.Mana = Convert.ToInt32(mana.Value);
            pl.Endurance = Convert.ToInt32(endurance.Value);
            if (pl.Endurance >= 10 && l && (!(r || m))) // levé tlačítko - útko na blízko
            {               
                endurance.Value -= 10;                
                interact.Consume(10, endurance, "-");
                

                attack.close();
                showWeapon(pl.Weapon.icon);

            }

            if (pl.Mana >= 10 && r && (!(l || m))) //pravé tlačítko - útok na dálku
            {               
                mana.Value -= 10;
                interact.Consume(10, mana, "-");

                attack.fire(PgoesRight);
                showWeapon(pl.RangedWeapon.icon);
                
            }

            if (m && (!(r || l))) //prostřední tlačítko - bude obrana
            {
                c.p.shieldActive = true;
                showWeapon(pl.Shield.icon);
            }
        }
        */
        public void pickWeapon() // vybere útok podle mezerníku nebo scroolu
        {
            if (meele)
            {
                ranged = true;
                meele = false;
                c.plWpnMirror.Viewbox = pl.RangedWeapon.icon;
            }
            else if (ranged)
            {
                meele = true;
                ranged = false;
                c.plWpnMirror.Viewbox = pl.Weapon.icon;
            }
        }

        public void pickAttack2(bool l, bool r) //zaútočí
        {
            var endurance = c.endurance;
            var mana = c.mana;
            pl.Mana = Convert.ToInt32(mana.Value);
            pl.Endurance = Convert.ToInt32(endurance.Value);

            if(l && !r)
            {
                if(pl.Endurance >= 10 && meele)
                {
                    endurance.Value -= 10;
                    interact.Consume(10, endurance, "-");

                    attack.close();
                    showWeapon(pl.Weapon.icon);
                }

                if(pl.Mana >= 10 && ranged)
                {
                    mana.Value -= 10;
                    interact.Consume(10, mana, "-");

                    attack.fire(PgoesRight);
                    showWeapon(pl.RangedWeapon.icon);
                }
            }

            if (r && !l) //pravé tlačítko
            {
                c.p.shieldActive = true;
                showWeapon(pl.Shield.icon);
            }
        }

        private void showWeapon(Rect icon) //zobrazí item
        {
            interact.show(playerWeapon); //zobrazí
            playerWeapon.Fill = weaponBrush; //změní brush
            weaponBrush.Viewbox = icon; // nastaví pozici ve spritu
        }

        public void regenerate(ProgressBar pr, TextBlock consBox) //regenerace many/endur
        {
            if (pr.Value <= pr.Maximum - 5)
            {
                pr.Value += 5;
            }

            if (pr.Value >= pr.Maximum)
            {
                pr.Value = pr.Maximum;
            }

            if (pr.Value % 10 == 0 && !(pr.Value >= pr.Maximum))
            {
                interact.Consume(10, pr, "+");
            }
        }

        public void positionPlayer(double pX, double pY) // pozicuje popostavičku podle polohy myši
        {
            playerBasicPositions(pX, pY);
            //posunutí na novou pozici - pro zavonání na myš
            Canvas.SetLeft(playerControl, pX - 25);
            Canvas.SetTop(playerControl, pY - 160);
        }

        public void positionPlayer2(double pX, double pY) // pozicuje popostavičku podle polohy myši - spomaleně
        {
            myX = pX - 25;
            myY = pY - 160;

            followMouse.Start();

            speed = pl.speed;

            playerBasicPositions(Canvas.GetLeft(playerControl), Canvas.GetTop(playerControl));
        }

        //pohyb šipkami - i šikmo
        //xml detekce - KeyDown="KeyDownHandler"      

        public void moveByKeys(KeyEventArgs e)
        {
            UIElement element = playerControl;

            if (element != null)
            {
                double left = Canvas.GetLeft(element);
                if (Double.IsNaN(left)) left = 0;
                double top = Canvas.GetTop(element);
                if (Double.IsNaN(top)) top = 0;

                speed = pl.speed;

                //jaká drží tlačítka
                bool l = Keyboard.IsKeyDown(Key.A); 
                bool r = Keyboard.IsKeyDown(Key.D);
                bool u = Keyboard.IsKeyDown(Key.W);
                bool d = Keyboard.IsKeyDown(Key.S);

                //pokud by mohl přesáhnout hranici dané strany tak m zakáže pohyb
                if (left <= speed) { l = false; }                
                if (top <= speed) { u = false; }
                if (left + playerControl.ActualWidth + speed >= c.fight_canvas.ActualWidth) { r = false; }
                if (top + playerControl.ActualHeight + speed >= c.fight_canvas.ActualHeight) { d = false; }

                move(l, r, u, d, left, top);

                playerBasicPositions(left, top);
            }
        }

        private void followingMouse(object sender, EventArgs e) // automaitcké dobíhání za myší
        {
            var animationControl = ImageBehavior.GetAnimationController(playerControl);

            if (!moves)
            {
                animationControl.Play();
            }

            double left = Canvas.GetLeft(playerControl);
            double top = Canvas.GetTop(playerControl);

            playerBasicPositions(left,top );//načte pozice         

            bool l = myX < left;
            bool r = myX > left;
            bool u = myY < top;
            bool d = myY > top;

            bool reachedY = top  - speed < myY && myY  < top + speed;
            bool reachedX = left - speed < myX && myX  < left + speed;

            if (left <= speed || reachedX) { l = false; }
            if (top <= speed || reachedY) { u = false; }
            if (left + playerControl.ActualWidth + speed >= c.fight_canvas.ActualWidth || reachedX) { r = false; }
            if (top + playerControl.ActualHeight + speed >= c.fight_canvas.ActualHeight || reachedY) { d = false; }

            move(l,r,u,d,left,top);

            if (reachedY && reachedX) //zastaví pohyb pokuď hráč dojde k myši
            {
                followMouse.Stop();
                animationControl.Pause();
                moves = false;
            }
           
        }

        private void move(bool l,bool r,bool u,bool d,double left,double top) //posouvání hráče na základě inputu
        {
            if (l && u) { left -= speed / 2; top -= speed / 2; }
            else if (l && d) { left -= speed / 2; top += speed / 2; }
            else if (r && u) { left += speed / 2; top -= speed / 2; }
            else if (d && r) { left += speed / 2; top += speed / 2; }
            else if (l) { left -= speed; }
            else if (r) { left += speed; }
            else if (u) { top -= speed; }
            else if (d) { top += speed; }

            //posunutí na novou pozici - pro zavonání na myš
            Canvas.SetLeft(playerControl, left);
            Canvas.SetTop(playerControl, top);
        }

        private void playerBasicPositions(double pX, double pY) //základní pozice hráče - akým čelí směrem pozice zbtaně
        {
            int Pelm_l = Convert.ToInt32(Canvas.GetLeft(playerControl));

            // správné natočení postavičky
            if (lastPx > pX)
            {
                if (!PgoesRight)
                {
                    interact.faceLeft(playerControl);
                    PgoesRight = true;
                }
            }

            if (lastPx < pX)
            {
                if (PgoesRight)
                {
                    interact.faceRight(playerControl);
                    PgoesRight = false;
                }
            }

            lastPx = pX; //uložení poslední pozice

            //posunutí obrázku zbraně
            if (!PgoesRight)
            {
                interact.faceLeft(playerWeapon);
                Canvas.SetLeft(playerWeapon, Canvas.GetLeft(playerControl) + 60);
            }
            else
            {
                interact.faceRight(playerWeapon);
                Canvas.SetLeft(playerWeapon, Canvas.GetLeft(playerControl) - 15);
            }

            Canvas.SetTop(playerWeapon, Canvas.GetTop(playerControl) + 17);

            //načte polohu hráče a zbraně 
            c.plRect = interact.ConvertToRect(playerControl);
            c.wpRect = interact.ConvertToRect(playerWeapon);
        }

    }
}
