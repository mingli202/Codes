using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows.Media;
using ImageLib;
using Sokoban.Objects;

namespace Sokoban.Interfaces
{
    public interface ICell
    {
        /// <summary>
        /// Draws this cell with its content. The cell is drawn as a square of side length "sideLength". The content is drawn on top of the cell.
        /// </summary>
        /// <param name="sideLength">Side length of the cell</param>
        public WorldImage DrawCell(int sideLength);

        /// <summary>
        /// Check if the content of this cell is a player.
        /// </summary>
        public bool ContainsPlayer();

        /// <summary>
        /// Return the position of this cell in the game board
        /// </summary>
        public Point Position();

        /// <summary>
        /// Check if it is possible to move to this cell.
        /// </summary>
        public bool CanMoveTo();

        /// <summary>
        /// Replace this cell's content with the given content and return the new cell.
        /// </summary>
        /// <param name="content">The content to replace the current content</param>
        public ICell ReplaceContent(IContent content);

        /// <summary>
        /// Check if this cell contains a trophy of the same color as the target (if this cell is a target).
        /// </summary>
        /// <returns></returns>
        public bool HasTrophyOfSameColor();

        /// <summary>
        /// Determines whether the current game state represents a win condition.
        /// </summary>
        public bool IsWon();


    }
}

