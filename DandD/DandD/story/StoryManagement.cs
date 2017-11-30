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

namespace DandD.story
{
    /// <summary>
    /// třída na základě levlu a výhry hráče určuje co se vypíše, jaké schpnosti pibydou hráčovy, nové itemy hráče, nového nepřítele
    /// </summary>
    class StoryManagement
    {

        private basicInteractions interact = new basicInteractions();
        private MainWindow c;

        private Player player;

        private int lastStatsImproved = 1;

        public StoryManagement()
        {
            c = interact.getContext();

            lastStatsImproved = c.lastIm;

            player = c.p;
        }

        public void gimmeStory() // určí příběh podle lvl
        {
            switch (player.LVL)
            {
                case 1:
                    c.enemy = new Zombie();
                    storyBuilder(
                        "You are now in wastelands.",
                        "In the distance you see a small zombie.",
                        "");

                    break;

                case 2:
                    c.enemy = new ZombieInSuit();
                    if (player.won)
                    {
                        addStats(100, 40, 60, 5, 3, 4, player.Weapon, player.items[3], player.Shield,1);
                    }

                    storyBuilder(
                        "You are now still in wastelands.",
                        "In the distance you see slightly bigger zombie that looks like a bussinessman with flesh hanging of him.",
                        "new shiny iron bow");

                    break;

                case 3:
                    c.enemy = new Blob();
                    if (player.won)
                    {
                        addStats(300, 90, 120, 6, 3, 6, player.items[0], player.RangedWeapon, player.items[5],2);
                    }

                    storyBuilder(
                           "You are now in swamp.",
                           "You see blob emerging from a puddle. Be aware of toutching him his slime is very poisonous you wont survive contact with him for very long.",
                           "iron sword and knights shield");

                    break;

                case 4:
                    c.enemy = new Spider();
                    if (player.won)
                    {
                        if (lastStatsImproved < player.LVL)
                        {
                            player.speed += 4;
                        }

                        addStats(600, 90, 120, 7, 3, 6, player.items[1], player.RangedWeapon, player.items[6],5);
                    }

                    storyBuilder(
                           "You are now in cave complex.",
                           "You see big spider.",
                           "Knights sword and Mythycal shield");                    

                        break;

                case 5:
                    c.enemy = new Dragon();
                    if (player.won)
                    {
                        addStats(1200, 90, 120, 7, 3, 6, player.items[2], player.items[4], player.items[6],10);
                    }

                    storyBuilder(
                           "You've gotten out of the cave back on to the light and you see dragons castle.",
                           "And there is enormous dragon",
                           "Poseidons Trident and Mythycal bow");

                    break;

                case 6:

                    c.storyText.Text = "You won the game!";
                    c.storyText.FontSize = 40;
                    interact.collapse(c.start_fight);

                    break;
            }
        }

        private void storyBuilder(string place,string monster, string recieved) //generováníé stringů pro příběh
        {
            if (player.fisrtGame)
            {
                c.storyText.Text = "Welcome " + player.Name + ", You have gone on epic quest to rescue your village from dragon,  during your quest you will encounter few monster which will try to kill you." + place +
                                    " " + monster + " To kill it press let's fight. In fight you can move with mouse use left mouse button to attack and right to defend yourself, you can switch between weapons by scrolling or with space bar";
            }
            else
            {
                if (player.won)
                {
                    c.storyText.Text = "Congratulations you beat that beast, " + place + " " + monster + " From last fight you recieved: " + recieved;
                }
                else
                {
                    c.storyText.Text = "Unfortunately you did not beat that beast. You can adjust your stats and try to kill monster then.";
                }                
            }
        }

        private void addStats(int hp, int mana, int endurance, int strenght, int armor,int critChance, Item wp,Item ranged,Item shd,int xp) //měnící se hodnoty při levelování
        {
            if(lastStatsImproved < player.LVL)
            {
                player.HP += hp;
                player.maxHP += hp;
                player.Mana += mana;
                player.manaMax += mana;
                player.Endurance += endurance;
                player.enduranceMax += endurance;
                player.Strenght += strenght;
                player.Armor += armor;
                player.CritChance += critChance;
                player.Weapon = wp;
                player.RangedWeapon = ranged;
                player.Shield = shd;
                player.XP += xp;

                c.lastIm = player.LVL;
                lastStatsImproved = player.LVL;
            }          
        }

    }
}
