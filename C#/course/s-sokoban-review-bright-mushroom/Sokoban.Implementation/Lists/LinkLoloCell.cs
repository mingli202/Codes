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
    public class LinkLoloCell : ILoloCell
    {
        readonly ILoCell first;
        readonly ILoloCell rest;

        public LinkLoloCell(ILoCell first, ILoloCell rest)
        {
            this.first = first;
            this.rest = rest;
        }

        public WorldImage DrawLoLoCell(int cellSideLength)
        {
            return new AboveAlignImage(AlignModeX.Left,
                first.DrawLoCell(cellSideLength),
                rest.DrawLoLoCell(cellSideLength)
                );
        }

        public Point GetPlayerPosition()
        {
            return this.first.ContainsPlayer() ? this.first.GetPlayerPos() : this.rest.GetPlayerPosition();
        }

        public ILoloCell MovePlayer(Point oldPos, Point newPos)
        {
            if(!this.CanMoveTo(newPos))
            {
                return this;
            }

            return ReplaceCell(oldPos, new EmptyContent()).ReplaceCell(newPos, new Player());
        }


        public int Height()
        {
            return 1 + this.rest.Height();
        }

        public int Width()
        {
            return this.first.Length();
        }

        public bool CanMoveTo(Point pos)
        {
            if (pos.y < 0 || pos.y >= this.Height()) { return false; }

            return this.GetRow(pos.y).CanMoveTo(pos.x);
        }

        public ILoloCell ReplaceCell(Point pos, IContent content)
        {
            return new LinkLoloCell(
                pos.y == 0 ? this.first.ReplaceCell(pos.x, content) : this.first,
                this.rest.ReplaceCell(new Point(pos.x, pos.y - 1), content)
                );
        }

        public bool IsLevelWon()
        {
            return this.first.IsRowWon() && this.rest.IsLevelWon();
        }

        public ILoCell GetRow(int index)
        {
            if (index == 0)
            {
                return this.first;
            }
            return this.rest.GetRow(index - 1);
        }
    }
}

