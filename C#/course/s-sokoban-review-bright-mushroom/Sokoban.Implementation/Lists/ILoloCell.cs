using System;
using ImageLib;
using Sokoban.Interfaces;
using Sokoban.Objects;

namespace Sokoban.Lists
{
    public interface ILoloCell
    {
        /// <summary>
        /// Get the width of this list of cells (i.e. the number of cells in each row).
        /// </summary>
        public int Width();

        /// <summary>
        /// Get the height of this list of cells (i.e. the number of rows).
        /// </summary>
        public int Height();

        /// <summary>
        /// Get the position of the player in this list of cells as a point with x,y value
        /// </summary>
        public Point GetPlayerPosition();

        /// <summary>
        /// Move the player from the old position to the new position, and return a new list of lists of cells with the player moved.
        /// </summary>
        public ILoloCell MovePlayer(Point oldPos, Point newPos);

        /// <summary>
        /// Draw this list of lists of cells as a world image, where each cell is drawn as a square with the given side length.
        /// </summary>
        public WorldImage DrawLoLoCell(int cellSideLength);
        
        /// <summary>
        /// Determines whether movement to the specified position is allowed.
        /// </summary>
        public bool CanMoveTo(Point pos);

        /// <summary>
        /// Replace the content of the cell at the specified position with the provided content, and return a new list of lists of cells reflecting this change.
        /// </summary>
        public ILoloCell ReplaceCell(Point pos, IContent content);

        /// <summary>
        /// Retrieves the row of this grid at the given index.
        /// </summary>
        /// <param name="index">The index of the row we want to retrieve (top row is 0 and index increments with each row)</param>
        /// <returns></returns>
        public ILoCell GetRow(int index);

        /// <summary>
        /// Determines whether the level has been won by checking if all goals are satisfied.
        /// </summary>
        public bool IsLevelWon();
    }
}

