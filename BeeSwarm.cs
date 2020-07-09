using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FlyingBee
{
    class BeeSwarm
    {
        Bee [] bees= new Bee[4];
        Random random = new Random();
        public int Length { get; private set; }
        public Point InitialLocation { get; private set; }
        private Point destination;

        public Point Destination
        {
            get { return destination; }
            set 
            {
                destination = value;
                for (int i = 0; i < bees.Length; i++)
                {
                    bees[i].DestinationReached = false;
                    int randomX = random.Next(value.X - 20, value.X + 20);
                    int randomY = random.Next(value.Y - 20, value.Y + 20);
                    bees[i].Destination = new Point(randomX, randomY);

                    if (bees[i].Destination.X <= bees[i].CurrentLocation.X)
                        bees[i].Orientation = MoveOrientation.Left;
                    else
                        bees[i].Orientation = MoveOrientation.Right;
                }
            }
        }

        public BeeSwarm(int x, int y)
        {
            InitialLocation = new Point((int)Math.Floor(x - x * 0.1), (int)Math.Floor(y * 0.1));
            for (int i = 0; i < bees.Length; i++)
            {
                bees[i] = new Bee();
                bees[i].Ordinal = i;
                bees[i].InitialLocation = new Point(random.Next(InitialLocation.X - 50, InitialLocation.X + 50), random.Next(InitialLocation.Y - 50, InitialLocation.Y + 50));
                bees[i].CurrentLocation = bees[i].InitialLocation;
                bees[i].DestinationReached = false;
                bees[i].Flipped = false;
                bees[i].Orientation = MoveOrientation.Left;
            }

            Length = bees.Length;  
        }

        public Bee this[int i]
        {
            get { return bees[i]; }
        }

        public void DeltaMove()
        {
            //Destination is Point, struct with default value{X=0, Y=0}
            if (Destination.X != 0 && Destination.Y != 0)
            {
                for (int i = 0; i < bees.Length; i++)
                {
                    if (!bees[i].DestinationReached)
                    {
                        bees[i].CurrentLocation = Next(i);
                    }
                }
            }
        }

        private Point Next(int i)
        { 
            //Stop criteria
            if (Math.Abs(bees[i].CurrentLocation.X - bees[i].Destination.X) <= 1
                && Math.Abs(bees[i].CurrentLocation.Y - bees[i].Destination.Y) <= 1)
            {
                bees[i].DestinationReached = true;
                return bees[i].CurrentLocation;
            }
            else 
            {
                return NextLinear(i);
            }
        }

        private Point NextLinear(int i)
        {
            int deltaX;
            float deltaY, denominator = bees[i].Destination.X * 1.0F - bees[i].CurrentLocation.X;
            int delta = random.Next(1, 10);

            //Calculate deltaX depending on the orientation
            if (bees[i].CurrentLocation.X > bees[i].Destination.X)
                deltaX = bees[i].CurrentLocation.X - delta;
            else
                deltaX = bees[i].CurrentLocation.X + delta;

            //Calculate deltaY depending on the deltax(N.B. Division with zero is not allowed)
            if (Math.Abs(denominator) <= 1)
                return bees[i].CurrentLocation;
            else
                deltaY = (bees[i].Destination.Y - bees[i].CurrentLocation.Y) / denominator * (deltaX - bees[i].CurrentLocation.X) + bees[i].CurrentLocation.Y;

            return new Point(deltaX, (int)Math.Round(deltaY));
        }
    }
}
