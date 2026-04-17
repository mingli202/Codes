using ImageLib;
using Sokoban.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using ImageLib.Enumerations;
using System.Windows.Media;
namespace Sokoban.Objects
{
    public class EmptyContent : AContent
    {
        public override WorldImage DrawContent(int size)
        {
            return new RectangleImage(0,0, OutlineMode.Outline, Colors.Transparent);
        }

        public override bool IsEmpty()
        {
            return true;
        }
    }
}

