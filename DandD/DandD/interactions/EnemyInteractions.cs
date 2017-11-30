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
    class EnemyInteractions
    {
        private basicInteractions interact = new basicInteractions();
        private MainWindow c;

        public EnemyAttack attack;

        private bool goesRight;
        private bool moves = true; //nepřítel se pohybuje

        // maximální pozice na canvasu na které se smí vyskytovat nepřítel  - automaticky přispůsobena v kódu
        private int max_left = 100;
        private int max_top = 100;

        // cílová pozice nepřítele
        private int target_left = 50;
        private int target_top = 350;

        private int enWidth = 100;
        private int enHeight = 100;

        private Image enemyControl;

        private IEnemy enemy;

        public EnemyInteractions(IEnemy en)
        {
            c = interact.getContext();

            enemy = en;
            enemyControl = c.enemyControl;
            goesRight = c.goesRight;
            attack = new EnemyAttack(en);
            c.fightTimer.Interval = enemy.AttackSpeed;
        }

        public void Move() //random pohyb nepřítele a jeho rozhodování
        {
            c.randomMovemet.Interval = c.enemy.MovementSpeed; //obnoví starý cyklus pokud předtím stál na místě
            var animationControl = ImageBehavior.GetAnimationController(enemyControl);

            if (!moves)
            {
                animationControl.Play();
            }

            int new_l = 0;
            int new_t = 0;

            int elm_l = Convert.ToInt32(Canvas.GetLeft(enemyControl));
            int elm_t = Convert.ToInt32(Canvas.GetTop(enemyControl));

            //posunutí nepřítele podle cílové pozice
            if (elm_l < target_left)
            {
                new_l += 5;

                if (goesRight)
                {
                    interact.faceRight(enemyControl);
                    goesRight = false;
                }
            }
            else if (elm_l > target_left)
            {
                new_l -= 5;

                if (!goesRight)
                {
                    interact.faceLeft(enemyControl);
                    goesRight = true;
                }
            }
            else if (elm_t < target_top) // bez else bude chodit šikmo s else pouze diagonálně
            {
                new_t += 5;
            }
            else if (elm_t > target_top)
            {
                new_t -= 5;
            }

            //pokud dosáhl svého cíle
            if ((elm_t == target_top) && (elm_l == target_left))
            {
                // /5 a *5 slouží k zarovnní hodnot aby se nepřítel neseknul

                enWidth = (Convert.ToInt32(c.enRect.Width) / 5) * 5;
                enHeight = (Convert.ToInt32(c.enRect.Height) / 5) * 5;

                max_left = Convert.ToInt32(Math.Floor(c.fight_area.ActualWidth));
                max_top = Convert.ToInt32(Math.Floor(c.fight_area.ActualHeight));

                c.randomMovemet.Interval = c.enRestTime; //zastaví nepřítele na místě
                animationControl.Pause();
                moves = false;

                Random r = new Random();

                // náasobení a dělení pěti zarovná hodnoty aby jich mohl nepřítel dosáhnout
                target_left = (r.Next(enWidth, max_left) / 5) * 5 - enWidth;
                target_top = (r.Next(enHeight, max_top) / 5) * 5 - enHeight;
            }

            //dosazeí pozic nepřítele
            c.enRect = interact.ConvertToRect(enemyControl);

            Canvas.SetLeft(enemyControl, elm_l + new_l);
            Canvas.SetTop(enemyControl, elm_t + new_t);

            //spustí kotrolu zda může útočit
            c.autoAttack();

            //pokud by nepřítel mohl trefit hráče tak vystřelí
            if (c.plRect.Top <= c.enRect.Top + enWidth && c.plRect.Top >= c.enRect.Top && (c.plRect.Left < c.enRect.Left && goesRight || c.plRect.Left > c.enRect.Left && !goesRight))
            {
                attack.fire(goesRight);
            }
        }

    }
}
