using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using ImageLib;

namespace Sokoban.Interfaces
{
    public abstract class AContent : IContent
    {
        public abstract WorldImage DrawContent(int size);

        public virtual bool IsPlayer() => false;

        public virtual WorldImage ScaleImageToSize(string filePath, int size)
        {
            WorldImage boxImage = new FromFileImage(filePath);

            double boxWidth = boxImage.Width;

            double ratio = (double)size / boxWidth;

            return new ScaleImage(boxImage, ratio);
        }

        public virtual bool IsEmpty() => false;

        public virtual bool IsTrophy() => false;

        public virtual bool HasSameColor(Color otherColor) => false;
    }
}

