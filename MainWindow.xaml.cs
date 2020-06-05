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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Asteroids_T21_B
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Spielobjekte> AlleSpielobjekte = new List<Spielobjekte>();
        List<Asteroid> Asteroiden = new List<Asteroid>();
        Raumschiff Enterprise;
        List<Torpedo> Torpedos = new List<Torpedo>();

        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(17);
            //timer.Start();
            timer.Tick += Animate;

            
        }

        private void Animate(object sender, EventArgs e)
        {
            Zeichenfläche.Children.Clear();
            List<Torpedo> zuLöschendeTorpedos = new List<Torpedo>();
            List<Asteroid> zuLöschendeAsteroiden = new List<Asteroid>();

            bool verloren = false;

            foreach (Spielobjekte item in AlleSpielobjekte)
            {
                if(item.Move(Zeichenfläche, timer.Interval) && item is Torpedo)
                {
                    zuLöschendeTorpedos.Add((Torpedo)item);
                }
            }
            foreach (Asteroid A in Asteroiden)
            {
                foreach (Torpedo T in Torpedos)
                {
                    if (A.EnthältPunkt(T.x, T.y))
                    {
                        zuLöschendeAsteroiden.Add(A);
                        zuLöschendeTorpedos.Add(T);
                    }
                }
                
                if (A.EnthältPunkt(Enterprise.x, Enterprise.y))
                {
                    zuLöschendeAsteroiden.Add(A);
                    verloren = true;                    
                }
            }

            if(Asteroiden.Count==0)
            {
                MessageBoxResult ergebnis;
                ergebnis = MessageBox.Show("Sie haben gewonnen, nochmal?", "Game Over", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (ergebnis == MessageBoxResult.Yes)
                {
                    SpielStarten();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }

            if (verloren)
            {
                MessageBoxResult ergebnis;
                ergebnis = MessageBox.Show("Sie haben verloren, nochmal?", "Game Over", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (ergebnis == MessageBoxResult.Yes)
                {
                    SpielStarten();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
            
            Torpedos.RemoveAll(t => zuLöschendeTorpedos.Contains(t));
            Asteroiden.RemoveAll(a => zuLöschendeAsteroiden.Contains(a));
            AlleSpielobjekte.RemoveAll(x => zuLöschendeTorpedos.Contains(x) ||  zuLöschendeAsteroiden.Contains(x));

            AlleSpielobjekte.ForEach(s => s.Draw(Zeichenfläche));
        }

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            Btn_Start.IsEnabled = false;
            SpielStarten();
        }

        public void SpielStarten()
        {
            AlleSpielobjekte.Clear();
            Asteroiden.Clear();
            Torpedos.Clear();

            for (int i = 0; i < 10; i++)
            {
                Asteroiden.Add(new Asteroid(Zeichenfläche));
            }
            Enterprise = new Raumschiff(Zeichenfläche);

            AlleSpielobjekte.Add(Enterprise);
            AlleSpielobjekte.AddRange(Asteroiden);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (timer.IsEnabled)
            {
                switch (e.Key)
                {
                    case Key.Down:
                    case Key.S:
                        Enterprise.Beschleunige(false);
                        break;

                    case Key.Up:
                    case Key.W:
                        Enterprise.Beschleunige(true);
                        break;

                    case Key.Left:
                    case Key.A:
                        Enterprise.Lenke(true);
                        break;

                    case Key.Right:
                    case Key.D:
                        Enterprise.Lenke(false);
                        break;

                    case Key.Space:
                        Torpedos.Add(new Torpedo(Enterprise));
                        AlleSpielobjekte.Add(Torpedos.Last());
                        break;
                }
            }
        }
    }
}
