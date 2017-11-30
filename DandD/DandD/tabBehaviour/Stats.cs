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

namespace DandD.tabBehaviour
{
    class Stats
    {
        /*
         CHOVÁNÍ    
         - hráč má k dispozici množství bodů které může rozdělit mezi své schopnosti - návrh
             */

        private basicInteractions interact = new basicInteractions();
        private MainWindow c;

        public Stats() //nastavení obsahu editace statů - nedokončené
        {
            c = interact.getContext();

            c.randomMovemet.Stop();
            c.fightTimer.Stop();
            c.fight.IsEnabled = false;
            interact.collapse(c.fight);                     

            setImg(c.closeIc, c.p.Weapon.icon);
            setImg(c.rangedIc, c.p.RangedWeapon.icon);
            setImg(c.shieldIc, c.p.Shield.icon);

            c.hpText.Text = c.p.maxHP.ToString();
            c.critText.Text = c.p.CritChance.ToString();
            c.strenghtText.Text = c.p.Strenght.ToString();

            c.xp.Content = c.p.XP.ToString();

            checkXP();

        }

        private void setImg(Rectangle rc,Rect ic)
        {
            ImageBrush wpBr = new ImageBrush();
            wpBr.ViewboxUnits = BrushMappingMode.Absolute;
            wpBr.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/items/items2.png"));

            wpBr.Viewbox = ic; // nastaví pozici ve spritu
            rc.Fill = wpBr; //změní brush
        }

        private void manageUpDown(bool added)
        {                                  
            if(added)
            {
                c.p.XP--;
            }
            else
            {
                c.p.XP++;
            }

            checkXP();
            c.xp.Content = c.p.XP.ToString();
        }

        private void checkXP()
        {
            if (c.p.XP > 0)
            {
                interact.enable(c.strenghtPlus);
                interact.enable(c.critPlus);
                interact.enable(c.hpPlus);
            }
            else if (c.p.XP <= 0)
            {
                interact.disable(c.strenghtPlus);
                interact.disable(c.critPlus);
                interact.disable(c.hpPlus);
            }
        }

        public void hp(bool add)
        {
            double txt;
            if (add)
            {
                txt = Convert.ToDouble(c.hpText.Text) + 20;
            }
            else
            {
                txt = Convert.ToDouble(c.hpText.Text) - 20;
            }
            
            if (txt > 40 && txt < 5000)
            {
                c.hpText.Text = txt.ToString();
                manageUpDown(add);

                c.p.maxHP = Convert.ToInt32(txt);
            }
        }

        public void crit(bool add)
        {
            double txt;
            if (add)
            {
                txt = Convert.ToDouble(c.critText.Text) + 0.5;
            }
            else
            {
                txt = Convert.ToDouble(c.critText.Text) - 0.5;
            }

                if (txt > 0 && txt < 99)
            {
                c.critText.Text = txt.ToString();
                manageUpDown(add);

                c.p.CritChance = txt;
            }
        }

        public void strenght(bool add)
        {
            double txt;
            if (add)
            {
                txt = Convert.ToDouble(c.strenghtText.Text) + 1;
            }
            else
            {
                txt = Convert.ToDouble(c.strenghtText.Text) - 1;
            }

            if (txt > 0 && txt < 3000)
            {
                c.strenghtText.Text = txt.ToString();
                manageUpDown(add);

                c.p.Strenght = Convert.ToInt32(txt);
            }
        }

    }
}
