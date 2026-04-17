using ImageLib;
using Sokoban.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sokoban.Objects
{
    public class Wall : AContent
    {
        public Wall()
        {
        }

        public override WorldImage DrawContent(int size)
        {
            return ScaleImageToSize("./Assets/Wall.png", size);
        }
    }
}

