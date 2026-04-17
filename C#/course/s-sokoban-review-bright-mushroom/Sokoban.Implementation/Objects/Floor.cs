using ImageLib;
using Sokoban.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using ImageLib.Enumerations;
using System.Windows.Media;

namespace Sokoban.Objects
{
    public class Floor : ACell
    {
        public Floor(IContent content, Point point) : base(content, point) { }

        public override WorldImage DrawCell(int sideLength)
        {
            return new OverlayImage(this.content.DrawContent(sideLength), new RectangleImage(sideLength, sideLength, OutlineMode.Fill, Colors.AntiqueWhite));
        }

        public override ICell ReplaceContent(IContent content)
        {
            return new Floor(content, this.position);
        }
    }
}

