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

namespace DandD
{
    /// <summary>
    /// třída s hráčem a jeho vlastnostmi iconami itemy
    /// </summary>
    public class Player
    {
        private basicInteractions interact = new basicInteractions();
        
        public int HP = 100;
        public int maxHP = 100;
        public int Mana = 100;
        public int manaMax = 100;
        public int Endurance = 100;
        public int enduranceMax = 100;
        public int speed = 7;
        public int XP = 5;
        public bool won = false;
        public bool fisrtGame = true;
        private int _Strenght = 2;
        public int Armor = 1;
        public bool shieldActive = false;
        private int _LVL = 1;
        private double _CritChance = 8; //ze sta - 8%
        
        public string Name;

        public ImageSource skin;
        
        public List<Item> items = new List<Item>();
        public Item Weapon = new Item("Wooden sword", "close", 3, 0,true, new Rect(225, 225, 45, 45));
        public Item RangedWeapon = new Item("Wooden bow", "ranged", 6, 0, true, new Rect(45, 500, 45, 45));
        public Item Shield = new Item("Wooden shield", "shield", 0, 33, true, new Rect(0, 545, 45, 45));

        public Player()
        {
            items.Add(new Item("Iron sword", "close", 4, 0, false, new Rect(270, 225, 45, 45)));    //0
            items.Add(new Item("Knight's sword", "close", 10, 0, false, new Rect(315, 225, 45, 45)));//1 
            items.Add(new Item("Trident", "close", 120, 0, false, new Rect(270, 360, 45, 45)));      //2
            items.Add(new Item("Iron bow", "ranged", 9, 0, false, new Rect(0, 500, 45, 45)));       //3
            items.Add(new Item("Mythycal bow", "ranged", 60, 0, false, new Rect(225, 500, 45, 45)));//4
            items.Add(new Item("Knight's shield", "shield", 0, 60, false, new Rect(45, 545, 45, 45)));//5
            items.Add(new Item("Mythycal shield", "shield", 0, 80, false, new Rect(450, 545, 45, 45)));//6
        }
       
        public double CritChance { get { if (_CritChance >= 100) { _CritChance = 99; } return _CritChance; } set {  _CritChance = value; } }

        public int Strenght
        {
            get
            {

                Random rn = new Random();
                int decide = rn.Next(0,100);
                int crit = 0;

                if (decide > 100 - _CritChance)
                {
                    crit = rn.Next(4, 5 * _Strenght);
                }
                
                return _Strenght + crit;
            }

            set
            {
                _Strenght = value;
            }
        }

        public int LVL
        {
            get
            {
                return _LVL;
            }

            set
            {
                _LVL = value;
            }
        }
        
    }
}
