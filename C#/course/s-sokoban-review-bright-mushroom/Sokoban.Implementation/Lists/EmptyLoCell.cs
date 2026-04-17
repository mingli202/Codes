using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;
using Sokoban.Interfaces;
using Sokoban.Objects;

namespace Sokoban.Lists
{
    public class EmptyLoCell : ILoCell
    {
        public bool CanMoveTo(int x)
        {
           return false;
        }

        public bool ContainsPlayer()
        {
            return false;
        }

        public WorldImage DrawLoCell(int sideLength)
        {
            return new RectangleImage(0, 0, OutlineMode.Outline, Colors.Transparent);
        }

        public Point GetPlayerPos()
        {
            return new Point(-1, -1);
        }

        public bool IsRowWon()
        {
            return true;    
        }

        public int Length()
        {
            return 0;
        }

        public ILoCell ReplaceCell(int idx, IContent content)
        {
            return this;
        }
    }
}

