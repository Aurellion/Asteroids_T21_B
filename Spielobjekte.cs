using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Asteroids_T21_B
{
    abstract class Spielobjekte
    {
        public double x { get; private set; }
        public double y { get; private set; }
        public double vx { get; protected set; }
        public double vy { get; protected set; }
        

        public Spielobjekte(double x, double y, double vx, double vy)
        {
            this.x = x;
            this.y = y;
            this.vy = vy;
            this.vx = vx;
        }

        abstract public void Draw(Canvas Zeichenfläche);
        public bool Move(Canvas Zeichenfläche, TimeSpan interval)
        {
            bool rausgeflogen = false;
            x += vx * interval.TotalSeconds;
            y += vy * interval.TotalSeconds;

            if (x > Zeichenfläche.ActualWidth)
            {
                x = 0;
                rausgeflogen = true;
            }
            if (x < 0)
            {
                x = Zeichenfläche.ActualWidth;
                rausgeflogen = true;
            }
            if (y > Zeichenfläche.ActualHeight)
            {
                y = 0;
                rausgeflogen = true;
            }
            if (y < 0)
            {
                y = Zeichenfläche.ActualHeight;
                rausgeflogen = true;
            }
            return rausgeflogen;
        }

        
    }

    class Asteroid : Spielobjekte
    {
        static Random rnd = new Random();
        Polygon umriss = new Polygon();
        public Asteroid(Canvas Zeichenfläche) 
            : base(rnd.NextDouble() * Zeichenfläche.ActualWidth,
                   rnd.NextDouble() * Zeichenfläche.ActualHeight,
                   (rnd.NextDouble() - 0.5) * 200,
                   (rnd.NextDouble() - 0.5) * 200)
        {
            for (int i = 0; i < 20; i++)
            {
                double radius, winkel;
                radius = 7 * rnd.NextDouble() + 15;
                winkel = 2 * Math.PI / 20 * i;
                umriss.Points.Add(new Point(radius * Math.Cos(winkel),
                                            radius * Math.Sin(winkel)));
                umriss.Fill = Brushes.Gray;
            }
        }

        public bool EnthältPunkt(double x, double y)
        {
            return umriss.RenderedGeometry.FillContains(new Point(x-this.x, y-this.y));
        }

        public override void Draw(Canvas Zeichenfläche)
        {
            Zeichenfläche.Children.Add(umriss);
            Canvas.SetTop(umriss, y);
            Canvas.SetLeft(umriss, x);
        }

       
    }

    class Raumschiff : Spielobjekte
    {
        Polygon umriss = new Polygon();

        public Raumschiff(Canvas Zeichenfläche) 
            : base(0.5*Zeichenfläche.ActualWidth,
                   0.5*Zeichenfläche.ActualHeight,
                   0.01,
                   -1)
        {
            umriss.Points.Add(new Point(0,-10));
            umriss.Points.Add(new Point(5,7));
            umriss.Points.Add(new Point(-5,7));
            umriss.Fill = Brushes.Blue;
        }
        /// <summary>
        /// Lenkt das Raumschiff nach Links für true und nach Rechts für false.
        /// </summary>
        /// <param name="nachLinks"></param>
        public void Lenke(bool nachLinks)
        {
            double winkel = (nachLinks ? -5 : 5) * Math.PI / 180;
            double sin = Math.Sin(winkel);
            double cos = Math.Cos(winkel);
            double vxn, vyn;
            vxn = cos * vx - sin * vy;
            vyn = sin * vx + cos * vy;
            vx = vxn;
            vy = vyn;
        }

        public override void Draw(Canvas Zeichenfläche)
        {
            double winkel = Math.Atan2(vy, vx) * 180 / Math.PI + 90;
            umriss.RenderTransform = new RotateTransform(winkel);
            Zeichenfläche.Children.Add(umriss);
            Canvas.SetTop(umriss, y);
            Canvas.SetLeft(umriss, x);
        }

        public void Beschleunige(bool beschleunige)
        {
            double faktor = beschleunige ? 1.1 : 0.9;
            vx *= faktor;
            vy *= faktor;
        }
    }

    class Torpedo : Spielobjekte
    {
        public Torpedo(Raumschiff Schiff)
            : base(Schiff.x, Schiff.y, 2*Schiff.vx, 2*Schiff.vy )
        {

        }
        public override void Draw(Canvas Zeichenfläche)
        {
            Ellipse e = new Ellipse();
            e.Width = 5;
            e.Height = 5;
            e.Fill = Brushes.Red;
            Zeichenfläche.Children.Add(e);
            Canvas.SetLeft(e, x-0.5 * e.Width);
            Canvas.SetTop(e, y - 0.5 * e.Height);
        }
    }
}
