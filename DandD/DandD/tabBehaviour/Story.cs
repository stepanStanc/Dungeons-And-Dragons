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
using DandD.story;

namespace DandD.tabBehaviour
{
    class Story
    {
        
        public Story() // načte třídu pro spracování příběhu
        {         
            StoryManagement manageStory = new StoryManagement();

            manageStory.gimmeStory();
        }

    }
}
