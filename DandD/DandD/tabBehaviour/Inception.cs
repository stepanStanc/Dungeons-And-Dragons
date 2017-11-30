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
    class Inception
    {

        private basicInteractions interact = new basicInteractions();
        private MainWindow c;

        private Player player;

        public Inception(Player pl)
        {
            c = interact.getContext();

            player = pl;

            interact.hide(c.story);
            //interact.hide(c.equip);
            interact.hide(c.stats);
            interact.hide(c.fight);
            
        }

        public void chosen(int pick)
        {
            switch(pick)
            {
                case 1:
                    player.skin = new BitmapImage(new Uri("pack://application:,,,/images/hero/man.gif"));
                    nextStage();
                break;

                case 2:
                    player.skin = new BitmapImage(new Uri("pack://application:,,,/images/hero/man2.gif"));
                    nextStage();
                break;

                case 3:
                    player.skin = new BitmapImage(new Uri("pack://application:,,,/images/hero/bear.gif"));
                    nextStage();
                break;               
            }
        }

        private void nextStage()
        {
            interact.collapse(c.man);
            interact.collapse(c.man2);
            interact.collapse(c.bear);

            interact.collapse(c.introl_label1);
            interact.collapse(c.introl_label2);

            interact.show(c.intro_label3);
            interact.show(c.newName);
            interact.show(c.setName);
        }

        public void setName(string name)
        {
            player.Name = name;

            c.story.IsSelected = true; // vynuceně změní tab - spustí hru
            interact.show(c.story);
            //interact.show(c.equip);
            interact.show(c.stats);

            interact.collapse(c.inc);
            c.inc.IsEnabled = false;
        } 

    }
}
