using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using ImageLib;

namespace Sokoban.Interfaces
{
    public interface IContent
    {
        /// <summary>
        /// Draws the visual content at the specified size and returns it as a world image.
        /// </summary>
        /// <param name="size">The width and height, in pixels, of the resulting image. Must be a positive integer.</param>
        public WorldImage DrawContent(int size);

        /// <summary>
        /// Check if this IContent is a player
        /// </summary>
        public bool IsPlayer();

        /// <summary>
        /// Check if this IContent is empty
        /// </summary>
        public bool IsEmpty();

        /// <summary>
        /// Check if this IContent is a trophy
        /// </summary>
        public bool IsTrophy();

        /// <summary>
        /// Check if this IContent has the same color as the given color. 
        /// </summary>
        public bool HasSameColor(Color otherColor);
    }
}

