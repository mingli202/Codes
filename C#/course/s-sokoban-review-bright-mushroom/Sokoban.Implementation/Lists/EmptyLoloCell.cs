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
    public class EmptyLoloCell : ILoloCell
    {
        public WorldImage DrawLoLoCell(int cellSideLength)
        {
            return new RectangleImage(0, 0, OutlineMode.Outline, Colors.Transparent);
        }

        public Point GetPlayerPosition()
        {
            //Throw error?
            return new Point(-1, -1);
        }

        public ILoloCell MovePlayer(Point oldPos, Point newPos)
        {
            return this;
        }

        public int Height()
        {
            return 0;
        }

        public int Width()
        {
            return 0;
        }

        public bool CanMoveTo(Point pos)
        {
            return false;
        }

        public ILoloCell ReplaceCell(Point pos, IContent content)
        {
            return this;
        }

        public bool IsLevelWon()
        {
            return true;
        }

        public ILoCell GetRow(int index)
        {
            return new EmptyLoCell();
        }
    }
}

