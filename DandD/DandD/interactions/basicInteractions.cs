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

namespace DandD.interactions
{
    /// <summary>
    /// opakovaně používané funkce pro interakci s XAML
    /// </summary>
    class basicInteractions
    {

        private ScaleTransform goLeft = new ScaleTransform();
        private ScaleTransform goRight = new ScaleTransform();

        public basicInteractions() 
        {
            goLeft.ScaleX = -1;
            goRight.ScaleX = 1;
        }
       
        public MainWindow getContext() //načtení současného contextu MainWindow - umožní z jakékoliv funkce pracovat s MainWindow
        {
            return ((MainWindow)System.Windows.Application.Current.MainWindow);
        }
       
        public void fadeInOut(UIElement target) // double animace objevení a zmizení - rychlá
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            da.AutoReverse = true;

            target.BeginAnimation(UIElement.OpacityProperty, da);
        }
      
        public void Consume(int amount, ProgressBar bar, string op) // animace konzumace mana / endur
        {   
            var consumptionBox = getContext().consumptionBox;
            if (bar.Name == "mana")
            {
                consumptionBox.Foreground = Brushes.DarkBlue;
            }
            else
            {
                consumptionBox.Foreground = Brushes.Brown;
            }

            show(consumptionBox);
            consumptionBox.Text = op + amount.ToString();

            fadeInOut(consumptionBox);
        }
        
        public void Hit(int dmg, Image ctrl) //spracování ubrání životů hráč / enemy
        {
            var hitBox = getContext().hitBox;
            if (ctrl.Name == "enemyControl")
            {
                hitBox.Foreground = Brushes.Green;
            }
            else
            {
                hitBox.Foreground = Brushes.Red;
            }

            Canvas.SetLeft(hitBox, Canvas.GetLeft(ctrl) - 10);
            Canvas.SetTop(hitBox, Canvas.GetTop(ctrl) - 30);
            show(hitBox);
            hitBox.Text = "-" + dmg.ToString();
            fadeInOut(hitBox);
        }

        public Rect ConvertToRect(FrameworkElement elm) //vytvoří rect - s xml prvku WPF
        {
            return new Rect(Canvas.GetLeft(elm), Canvas.GetTop(elm), elm.ActualWidth, elm.ActualHeight); ;
        }

        public bool overlap(Rect rect1, Rect rect2) //překrývají se ?
        {
            return rect1.IntersectsWith(rect2);
        }

        public void hide(FrameworkElement elm) //skrytí
        {
            elm.Visibility = Visibility.Hidden;
        }

        public void show(FrameworkElement elm) //zobrazení
        {
            elm.Visibility = Visibility.Visible;
        }

        public void collapse(FrameworkElement elm) //zabalení - skryje i pozicování (graficky neexistuje)
        {
            elm.Visibility = Visibility.Collapsed;
        }

        public void faceLeft(FrameworkElement elm) //otočí do leva
        {
            elm.RenderTransform = goLeft;
        }

        public void faceRight(FrameworkElement elm) //otoč doprava
        {
            elm.RenderTransform = goRight;
        }

        public void disable(FrameworkElement elm) //vypne
        {
            elm.IsEnabled = false;
        }

        public void enable(FrameworkElement elm) //zabne
        {
            elm.IsEnabled = true;
        }




        public void FightEnded() //když souboj skončí
        {
            getContext().randomMovemet.Stop();
            getContext().fightTimer.Stop();
            getContext().fight.IsEnabled = false;
            collapse(getContext().fight);

            getContext().stats.IsEnabled = true;
            //c.equip.IsEnabled = true;
            getContext().story.IsEnabled = true;

            show(getContext().stats);
            //interact.show(c.equip);
            show(getContext().story);

            getContext().story.IsSelected = true;          
        }

    }
}
