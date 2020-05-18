using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Asteroids_T21_B
{
    abstract class Spielobjekte
    {
        public double x { get; private set; }
        public double y { get; private set; }
        protected double vx;
        protected double vy;

        public Spielobjekte(double x, double y, double vx, double vy)
        {
            this.x = x;
            this.y = y;
            this.vy = vy;
            this.vx = vx;
        }

        abstract public void Draw(Canvas Zeichenfläche);
        public void Move(Canvas Zeichenfläche, TimeSpan interval)
        {
            x += vx * interval.TotalSeconds;
            y += vy * interval.TotalSeconds;

            if (x > Zeichenfläche.ActualWidth) x = 0;
            if (x < 0) x = Zeichenfläche.ActualWidth;
            if (y > Zeichenfläche.ActualHeight) y = 0;
            if (y < 0) y = Zeichenfläche.ActualHeight;
        }

        
    }

    class Asteroid : Spielobjekte
    {
        static Random rnd = new Random();
        public Asteroid(Canvas Zeichenfläche) 
            : base(rnd.NextDouble() * Zeichenfläche.ActualWidth,
                   rnd.NextDouble() * Zeichenfläche.ActualHeight,
                   (rnd.NextDouble() - 0.5) * 200,
                   (rnd.NextDouble() - 0.5) * 200)
        {

        }

        public override void Draw(Canvas Zeichenfläche)
        {
            Ellipse umriss = new Ellipse();
            umriss.Width = 10;
            umriss.Height = 10;
            umriss.Fill = Brushes.Gray;
            Zeichenfläche.Children.Add(umriss);
            Canvas.SetTop(umriss, y);
            Canvas.SetLeft(umriss, x);
        }

       
    }

    //class Raumschiff : Spielobjekte
    //{

    //}

    //class Torpedos : Spielobjekte
    //{

    //}
}
