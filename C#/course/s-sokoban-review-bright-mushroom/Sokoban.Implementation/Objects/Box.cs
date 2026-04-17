using ImageLib;
using Sokoban.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sokoban.Objects
{
    public class Box : AContent
    {
        public override WorldImage DrawContent(int size)
        {
            return ScaleImageToSize("./Assets/Box.png",size);
        }
    }
}

