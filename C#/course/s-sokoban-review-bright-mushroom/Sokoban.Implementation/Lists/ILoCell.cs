using System;
using System.Collections.Generic;
using System.Text;
using ImageLib;
using Sokoban.Interfaces;
using Sokoban.Objects;

namespace Sokoban.Lists
{
    public interface ILoCell
    {
        /// <summary>
        /// Return the length of this list of cells.
        /// </summary>
        public int Length();

        /// <summary>
        /// Draw this list of cells as a world image.
        /// </summary>
        public WorldImage DrawLoCell(int sideLength);

        /// <summary>
        /// Get the position of the player in this list of cells as a Point
        /// </summary>
        public Point GetPlayerPos();

        /// <summary>
        /// Check if this List of cells contains a player
        /// </summary>
        public bool ContainsPlayer();

        /// <summary>
        /// Check if a cell at the given index can be moved to by the player. 
        /// </summary>
        public bool CanMoveTo(int x);

        /// <summary>
        /// Return a new list of cells where the cell at the given index is replaced by a new cell with the given content.
        /// </summary>
        public ILoCell ReplaceCell(int idx, IContent content);

        /// <summary>
        /// Check if all the cells in this list are won (i.e. if all the targets in this list have trophies of the same color on top of them).
        /// </summary>
        public bool IsRowWon();

    }
}

