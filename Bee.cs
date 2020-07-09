using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FlyingBee
{
    class Bee
    {
        public Point InitialLocation { get; set; }
        public Point CurrentLocation { get; set; }
        public Point Destination { get; set; }
        public int Ordinal { get; set; }
        public bool DestinationReached { get; set; }
        public MoveOrientation Orientation { get; set; }
        public bool Flipped { get; set; }
    }
}
