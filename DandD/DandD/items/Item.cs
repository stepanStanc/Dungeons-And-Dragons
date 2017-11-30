using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DandD.items
{
    public class Item
    {
        private string _Name;

        private string _Type;

        private int _Strenght;
        private int _Armor;

        private Rect _Icon;

        private bool _Acessible;

        public Item(string Name,string Type, int Strenght, int Armor,bool Acessible, Rect Icon)
        {
            _Name = Name;
            _Type = Type;
            _Strenght = Strenght;
            _Armor = Armor;
            _Icon = Icon;
            _Acessible = Acessible;

        }

        public string name { get { return _Name; } set { _Name = value; } }

        public string type { get { return _Type; } set { _Type = value; } }

        public int strenght { get { return _Strenght; } set { _Strenght = value; } }
        public int armor { get { return _Armor; } set { _Armor = value; } }

        public bool acessible { get { return _Acessible; } set { _Acessible = value; } }

        public Rect icon { get { return _Icon; } set { _Icon = value; } }
    }
}
