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
        List<Asteroid> Asteroiden = new List<Asteroid>();
        Raumschiff Enterprise;
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
            foreach (Asteroid item in Asteroiden)
            {
                item.Draw(Zeichenfläche);
                item.Move(Zeichenfläche, timer.Interval);
            }
            Enterprise.Draw(Zeichenfläche);
            Enterprise.Move(Zeichenfläche, timer.Interval);

        }

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            Btn_Start.IsEnabled = false;
            for (int i = 0; i < 10; i++)
            {
                Asteroiden.Add(new Asteroid(Zeichenfläche));
            }
            Enterprise = new Raumschiff(Zeichenfläche);
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
                }
            }
        }
    }
}
