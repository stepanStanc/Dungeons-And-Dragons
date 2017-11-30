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
    /// <summary>
    /// načte a spustí vše potřebné pro fight
    /// </summary>
    class Fight
    {

        private basicInteractions interact = new basicInteractions();
        private MainWindow c;

        private Image enemyControl;

        public Fight(IEnemy enemy,Player player)
        {
            c = interact.getContext();

            enemyControl = c.enemyControl;
            c.randomMovemet.Interval = enemy.MovementSpeed;

            c.randomMovemet.Start();

            c.fight.IsEnabled = true;
            interact.show(c.fight);
            c.stats.IsEnabled = false;
            //c.equip.IsEnabled = false;
            c.story.IsEnabled = false;

            interact.collapse(c.stats);
            //interact.collapse(c.equip);
            interact.collapse(c.story);

            c.manaRecovery.Start();
            c.enduranceRecovery.Start();

            c.enemyControl.Width = enemy.enWidth;

            c.en_hp.Maximum = enemy.maxHP;
            c.en_hp.Value = enemy.maxHP;
            enemy.HP = enemy.maxHP;

            c.hp.Maximum = player.maxHP;
            c.hp.Value = player.maxHP;
            player.HP = player.maxHP;

            c.mana.Maximum = player.manaMax;
            c.mana.Value = player.manaMax;
            player.Mana = player.manaMax;

            c.endurance.Maximum = player.enduranceMax;
            c.endurance.Value = player.enduranceMax;
            player.Endurance = player.enduranceMax;

            ImageSource enSource = enemy.Img;
            ImageBehavior.SetAnimatedSource(enemyControl, enSource);

            ImageSource plSource = player.skin;
            ImageBehavior.SetAnimatedSource(c.playerControl, plSource);

            interact.hide(c.playerWeapon);

            c.textureBrush.ImageSource = enemy.Background;

            player.won = false;

            PlayerIntercations plI = new PlayerIntercations();
            plI.pickWeapon();
            plI.pickWeapon();
            plI.pickAttack2(true,false);
            interact.hide(c.playerWeapon);
        }

    }
}
