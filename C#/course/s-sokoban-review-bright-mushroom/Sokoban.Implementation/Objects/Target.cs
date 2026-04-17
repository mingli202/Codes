using Sokoban.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;
using System.Windows.Automation.Text;

namespace Sokoban.Objects
{
    public class Target : ACell
    {
        readonly Color color;

        public Target(Color color, IContent content, Point point) : base(content, point)
        {
            this.color = color;
        }

        public override WorldImage DrawCell(int sideLength)
        {
            return new OverlayImage(this.content.DrawContent(sideLength), this.DrawTarget(sideLength),
                    new RectangleImage(sideLength, sideLength, OutlineMode.Fill, Colors.AntiqueWhite));
        }

        public WorldImage DrawTarget(int sideLength)
        {
            return new OverlayImage(new CircleImage(sideLength / 8, OutlineMode.Fill, color),
                new CircleImage(sideLength / 6, OutlineMode.Fill, Colors.AntiqueWhite),
                new CircleImage(sideLength/4, OutlineMode.Fill, color));
        }

        public override ICell ReplaceContent(IContent content)
        {
            return new Target(this.color, content, this.position);
        }

        public override bool HasTrophyOfSameColor()
        {
            return this.content.IsTrophy() && this.content.HasSameColor(this.color);
        }

        public override bool IsWon()
        {
            return this.HasTrophyOfSameColor();
        }
    }
}

