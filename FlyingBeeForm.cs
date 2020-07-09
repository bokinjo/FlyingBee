using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FlyingBee
{
    public partial class FlyingBeeForm : Form
    {
        Bitmap[] animatedBee = new Bitmap[4];
        Bitmap hive;
        Rectangle hiveRectangle = new Rectangle();
        Rectangle beeRectangle = new Rectangle();

        BeeSwarm swarm;

        public FlyingBeeForm()
        {
            InitializeComponent();

            // Hive
            hive = Properties.Resources.Hive;
            hive.RotateFlip(RotateFlipType.RotateNoneFlipX);
            hiveRectangle.Location = new Point(this.Size.Width - hive.Width, 0);
            hiveRectangle.Size = new Size(100, 100);

            // Bees
            for (int i = 0; i < animatedBee.Length; i++)
                animatedBee[i] = (Bitmap)
                    (Properties.Resources.ResourceManager.GetObject(String.Format("Bee_animation_{0}", i)));
            beeRectangle.Location = new Point(0, 0);
            beeRectangle.Size = new Size(40, 40);

            swarm = new BeeSwarm(this.Size.Width, this.Size.Height);
            MessageBox.Show("Left click to set destination, right click to send home.");
        }

        /* 
         * The form fires a Paint event every time it needs to redraw itself—like when it’s dragged
         * off the screen. One of the properties of its PaintEventArgs parameter is a Graphics object.
         * We will use that object to draw our graphics.
         * N.B. When you use the Paint event for all your graphics, you can turn on double buffered painting
         * by changing the DoubleBuffered Form property to True. This will remove the appearance of flickering.
         * */
        private void FlyingBeeForm_Paint(object sender, PaintEventArgs e)
        {
            /*
             * You don’t have to use a 'using' statement for the Graphics object since you didn’t create it,
             * so you don’t have to dispose it. The .NET framework takes care of it.
             * */
            Graphics g = e.Graphics;
            DrawImages(g);
            System.Threading.Thread.Sleep(150);
            this.Invalidate();
        }

        private void DrawImages(Graphics g)
        {
            g.DrawImage(hive, hiveRectangle);
            swarm.DeltaMove();
            for (int i = 0; i < swarm.Length; i++)
            {
                if (swarm[i].Orientation == MoveOrientation.Left && swarm[i].Flipped)
                {
                    animatedBee[i].RotateFlip(RotateFlipType.RotateNoneFlipX);
                    swarm[i].Flipped = false;
                }
                else if (swarm[i].Orientation == MoveOrientation.Right && !swarm[i].Flipped)
                {
                    animatedBee[i].RotateFlip(RotateFlipType.RotateNoneFlipX);
                    swarm[i].Flipped = true;
                }

                beeRectangle.Location = swarm[i].CurrentLocation;
                switch (swarm[i].Ordinal)
                {
                    case 0: g.DrawImage(animatedBee[0], beeRectangle); break;
                    case 1: g.DrawImage(animatedBee[1], beeRectangle); break;
                    case 2: g.DrawImage(animatedBee[2], beeRectangle); break;
                    case 3: g.DrawImage(animatedBee[3], beeRectangle); break;
                    case 4: g.DrawImage(animatedBee[2], beeRectangle); break;
                    default: g.DrawImage(animatedBee[1], beeRectangle); swarm[i].Ordinal = 0; break;
                }
                swarm[i].Ordinal++;
            }
        }

        private void FlyingBeeForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left)) swarm.Destination = e.Location;
            else swarm.Destination = swarm.InitialLocation;
        }
    }
}
