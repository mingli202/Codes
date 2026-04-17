using ImageLib;
using Sokoban.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Sokoban.Objects
{
    public class Trophy : AContent
    {
        readonly Color color;

        public Trophy(Color color)
        {
            this.color = color; 
        }

        public override WorldImage DrawContent(int size)
        {
            if (color == Colors.Red)
            {
                return ScaleImageToSize("./Assets/RedTrophy.png", size);
            }
            else if (color == Colors.Green)
            {
                return ScaleImageToSize("./Assets/GreenTrophy.png", size);
            }
            else if (color == Colors.Blue)
            {
                return ScaleImageToSize("./Assets/BlueTrophy.png", size);
            }
            else if (color == Colors.Yellow)
            {
                return ScaleImageToSize("./Assets/YellowTrophy.png", size);
            }
            else
            {
                throw new ArgumentException("Invalid color for trophy.");
            }
        }

        public override bool IsTrophy() => true;

        public override bool HasSameColor(Color otherColor) => color == otherColor;
    }
}

