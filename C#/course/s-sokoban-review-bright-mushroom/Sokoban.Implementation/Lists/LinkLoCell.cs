using ImageLib;
using Sokoban.Interfaces;
using Sokoban.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sokoban.Lists
{
    public class LinkLoCell : ILoCell
    {
        readonly ICell first;
        readonly ILoCell rest;

        public LinkLoCell(ICell first, ILoCell rest)
        {
            this.first = first;
            this.rest = rest;
        }
        public WorldImage DrawLoCell(int sideLength)
        {
            return new BesideImage(this.first.DrawCell(sideLength), this.rest.DrawLoCell(sideLength));
        }

        public int Length()
        {
            return 1 + this.rest.Length();
        }

        public Point GetPlayerPos()
        {
            if (this.first.ContainsPlayer())
            {
                return this.first.Position();
            }
            return this.rest.GetPlayerPos();
        }

        public bool ContainsPlayer()
        {
            return this.first.ContainsPlayer() || this.rest.ContainsPlayer();
        }

        public bool CanMoveTo(int x)
        {
            if (x == 0)
            {
                return this.first.CanMoveTo();
            }
            return this.rest.CanMoveTo(x - 1);
        }

        public ILoCell ReplaceCell(int idx, IContent content)
        {
            if (idx == 0)
            {
                return new LinkLoCell(first.ReplaceContent(content), this.rest);
            }
            else
            {
                return new LinkLoCell(this.first, this.rest.ReplaceCell(idx - 1, content));
            }
        }

        public bool IsRowWon()
        {
            return this.first.IsWon() && this.rest.IsRowWon();
        }
    }
}

