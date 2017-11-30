using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;
using MahApps.Metro.Controls;
using System.Windows.Threading;
using DandD.interactions;
using DandD.enemy;
using DandD.tabBehaviour;
using DandD.items;
using System.Windows.Media.Animation;

namespace DandD
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        // globální proměné s výchozímy hodnotami
        //zaolžení timerů
        public DispatcherTimer fightTimer = new DispatcherTimer(); //pasivní útok nepřítele
        public DispatcherTimer randomMovemet = new DispatcherTimer(); //pohyb nepřítele
        public DispatcherTimer projectileMovement = new DispatcherTimer(); //pohyb střely - hráč
        public DispatcherTimer enemyProjectileMovement = new DispatcherTimer(); //pohyb střely - nepřítel
        public DispatcherTimer clickDelay = new DispatcherTimer(); // zamezení okamžitého klikání hráči
        public DispatcherTimer enFireDelay = new DispatcherTimer(); // zamezení neustálého střílení nepřítelem
        public DispatcherTimer manaRecovery = new DispatcherTimer(); // automatické obnovování many
        public DispatcherTimer enduranceRecovery = new DispatcherTimer(); // automatické obnovování výdrže/endurance

        // kotrolní booly
        public bool goesRight = false; //nepřítel jde vpravo
        public bool PgoesRight = false; //hráč jde vpravo     


        // bool pro ověření zda může hráč/nepřítel znovy zaůtočit/vystřelit
        public bool unblocked = true;
        public bool enFireUnblocked = true;

        //**************** new Timespan( d, h, min, s, ms );
        public TimeSpan enSpeed = new TimeSpan(0, 0, 0, 0, 20); //rychlost nepřítele
        public TimeSpan enRestTime = new TimeSpan(0, 0, 0, 1); // doba odpočinku nepřítele (nehybnost)

        //obrázek zastupující projektil nepřítele a hráče       
        public Image enemyProjectile = new Image();
        public List<Image> plProjectiles = new List<Image>();

        // grafické prvky hry které jsou zastoupeny Rect pro kontrolu překrytí - zapotřebí určení kdy kdo může komu ubírat životy
        public Rect enRect = new Rect(0,0,0,0);
        public Rect plRect = new Rect(0,0,0,0);
        public Rect wpRect = new Rect(0, 0, 0, 0);
        public Rect plPrRect = new Rect(0, 0, 0, 0);
        public Rect enPrRect = new Rect(0, 0, 0, 0);

        public int lastIm = 1;

        public Player p = new Player();
        public IEnemy enemy = new Zombie();
        //konsrturktory použity po plném spuštění aplikce pro interakci s kódem
        basicInteractions interact;
        PlayerIntercations plInteract;
        EnemyInteractions enInteract;

        Fight fightB;
        //Equip equipB;
        Inception incB;
        Stats statsB;
        Story storyB;

        //  troubleshoot
        //MessageBoxResult result = MessageBox.Show("Controlní text - string");

        public MainWindow()
        {
            InitializeComponent();

            //nastavení timerů

            // prvotní nastavení timerů
            fightTimer.Tick += new EventHandler(fightTimer_Tick);
            fightTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);

            randomMovemet.Tick += new EventHandler(rnMovement_Tick);
            randomMovemet.Interval = enSpeed;

            projectileMovement.Tick += new EventHandler(projectileMovement_Tick);
            projectileMovement.Interval = new TimeSpan(0, 0, 0, 0, 6);

            enemyProjectileMovement.Tick += new EventHandler(enemyProjectileMovement_Tick);
            enemyProjectileMovement.Interval = new TimeSpan(0, 0, 0, 0, 4);

            manaRecovery.Tick += new EventHandler(recoverMana);
            manaRecovery.Interval = new TimeSpan(0, 0, 0, 1, 800);

            enduranceRecovery.Tick += new EventHandler(recoverEndurance);
            enduranceRecovery.Interval = new TimeSpan(0, 0, 0, 1, 0);

            clickDelay.Tick += new EventHandler(delayClick);
            clickDelay.Interval = new TimeSpan(0, 0, 0, 0, 350);

            enFireDelay.Tick += new EventHandler(delayEnFire);
            enFireDelay.Interval = new TimeSpan(0, 0, 0, 1, 700);

            inc.IsSelected = true;           

            plInteract = new PlayerIntercations();
            interact = new basicInteractions();
            enInteract = new EnemyInteractions(enemy);           

            //  stats.IsEnabled = false; // zakáže otevíráníé tabu 
            //  stats.IsSelected = true; // vynuceně změní tab


        }

    /// <summary>
    ///  Ovládání hry v jednotlivých tabech
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (inc.IsSelected) // při spuštění hry
            {
                incB = new Inception(p);
            }

            if (story.IsSelected) //příběh
            {
                storyB = new Story();               
            }
            /*
            if (equip.IsSelected) //výměna oblečených itemů
            {
                equipB = new Equip();
            }
            */
            if (stats.IsSelected) //editace statů
            {
                statsB = new Stats();
            }

            if (fight.IsSelected) //soubojová meachanika a spuštění
            {
                fightB = new Fight(enemy, p);
            }

        }

        /****************************************************************************************
         ********************* INTERAKCE XML ****************************************************
         ****************************************************************************************/

        /**********************************************************
         ********************* ZAČÁTEK HRY ************************
         **********************************************************/

        private void man_chosen(object sender, MouseButtonEventArgs e)
        {
            incB.chosen(1);
        }

        private void man2_chosen(object sender, MouseButtonEventArgs e)
        {
            incB.chosen(2);
        }

        private void bear_chosen(object sender, MouseButtonEventArgs e)
        {
            incB.chosen(3);
        }

        private void nameChosen(object sender, RoutedEventArgs e)
        {
            incB.setName(newName.Text);
        }

        
        /**********************************************************
         *********************  PRIBEH ************************
         **********************************************************/

        private void startFight(object sender, RoutedEventArgs e) // spustí souboj
        {
            story.IsSelected = false;
            fight.IsSelected = true;
            p.fisrtGame = false;
        }

        /**********************************************************
         *********************  STATY ************************
         **********************************************************/

        private void hpAdd(object sender, RoutedEventArgs e)
        {
            statsB.hp(true);
        }

        private void critAdd(object sender, RoutedEventArgs e)
        {
            statsB.crit(true);
        }

        private void strenghtAdd(object sender, RoutedEventArgs e)
        {
            statsB.strenght(true);
        }

        private void hpSub(object sender, RoutedEventArgs e)
        {
            statsB.hp(false);
        }

        private void critSub(object sender, RoutedEventArgs e)
        {
            statsB.crit(false);
        }

        private void strenghtSub(object sender, RoutedEventArgs e)
        {
            statsB.strenght(false);
        }

        /**********************************************************
         ********************* SOUBOJ *****************************
         **********************************************************/

        // funkce pro zobrazení a skrytí myši při boji
        private void enterEv(object sender, MouseEventArgs e)
        {           
            //Mouse.OverrideCursor = Cursors.None;
                       
            //custom kurzor
            Cursor paintBrush = new Cursor(Application.GetResourceStream(new Uri("images/target.cur", UriKind.Relative)).Stream);
            Mouse.OverrideCursor = paintBrush;
            
        }

        //zobrazí kurzor o opuštění bitevného pole
        private void leaveEv(object sender, MouseEventArgs e)
        {         
            
            Mouse.OverrideCursor = null;
        }

        //schová zbraň po puštění tlačítka myši
        private void MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            interact.hide(playerWeapon);
            p.shieldActive = false;
        }

        //útok a borana na kliknutí - l na blízkost, p na dálku , prostřední na obranu
        private void MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (unblocked)
            {
                //zapotřebí předem identifikovat pro detekci více tlačítek
                bool l = e.ChangedButton == MouseButton.Left;
                bool r = e.ChangedButton == MouseButton.Right;
                bool m = e.ChangedButton == MouseButton.Middle;

                //plInteract.pickAttack(l,r,m);
                plInteract.pickAttack2(l,r);

                clickDelay.Start(); // spustí pro znovusprovoznění lačítka
                unblocked = false; // zablokuje
            }

        }       

        //omezí počet kliknutí / s
        private void delayClick(object sender, EventArgs e)
        {
            unblocked = true;
            clickDelay.Stop();
        }

        //pohyb myší určuje phyb postavičky
        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {     
            System.Windows.Point position = e.GetPosition(this);
            plInteract.positionPlayer2(position.X, position.Y); // positionPlayer(int,int) - pro okamžitý pohyb myší   
        }

        // spuštění automatiického útoku
        public void autoAttack()
        {
            if (interact.overlap(plRect,enRect))
            {
                fightTimer.Start();
            }
            else
            {
                fightTimer.Stop();
            }
        }

        // passivní útok nepřítele
        private void fightTimer_Tick(object sender, EventArgs e)
        {
            int dmg = 1;

            if (p.shieldActive)
            {
                dmg = Convert.ToInt32(enemy.closeDmg() * (100 - p.Shield.armor) / 100);
            }
            else
            {
               dmg = enemy.closeDmg();
            }
            interact.fadeInOut(plTakenDmg);
            hp.Value -= dmg;
            p.HP -= dmg;
            interact.Hit(dmg, playerControl);
            if (p.HP <= 0)
            {
                interact.FightEnded();
            }
        }

        // pohyb nepřítele
        private void rnMovement_Tick(object sender, EventArgs e)
        {
            enInteract.Move();           
        }

        // pohyb projektilu hráče
        private void projectileMovement_Tick(object sender, EventArgs e)
        {
            //plInteract.attack.projectileMovementFc();
        }

        //omezení dálkových útoků nepřítelem
        private void delayEnFire(object sender, EventArgs e)
        {
            enFireUnblocked = true;
            enFireDelay.Stop();
        }

        //pohyb projektilu nepřítele
        private void enemyProjectileMovement_Tick(object sender, EventArgs e)
        {
            enInteract.attack.projectileMovementFc();            
        }

        // funkce pro obnovování many a výdrže
        private void recoverMana(object sender, EventArgs e)
        {
            plInteract.regenerate(mana,consumptionBox);
        }

        private void recoverEndurance(object sender, EventArgs e)
        {
            plInteract.regenerate(endurance, consumptionBox);
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (fight.IsSelected)
            {
                //plInteract.moveByKeys(e); //- pohyb klávesnicí
                if(e.Key == Key.Space) { plInteract.pickWeapon(); }
                    
            }
        }

        //změna zbraně při pohybu kolečka myši
        private void handleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            plInteract.pickWeapon();
        }
    }
}

