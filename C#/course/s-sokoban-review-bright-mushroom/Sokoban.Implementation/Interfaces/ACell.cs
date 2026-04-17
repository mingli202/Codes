using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using ImageLib;
using Sokoban.Objects;

namespace Sokoban.Interfaces
{
    public abstract class ACell : ICell
    {
        protected readonly Point position;
        protected readonly IContent content;

        public ACell(IContent content, Point point)
        {
            this.position = point;
            this.content = content;
        }

        public abstract WorldImage DrawCell(int sideLength);

        public virtual bool ContainsPlayer()
            => this.content.IsPlayer();

        public virtual Point Position()
        {
            return position;
        }

        public virtual bool CanMoveTo()
        {
            return this.content.IsEmpty();
        }

        public abstract ICell ReplaceContent(IContent content);

        public virtual bool HasTrophyOfSameColor()
            => false;

        public virtual bool IsWon()
            => true;
    }
}

