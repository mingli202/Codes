using ImageLib;
using Sokoban.Cells;
using Sokoban.Lists;

namespace Sokoban;

/// <summary>
/// Represents a board of cells.
/// </summary>
/// <param name="cells">The cell grid backing this board.</param>
public class Board(IListOf<IListOf<ICell>> cells) : Grid<ICell>(cells)
{
    /// <summary>
    /// Creates a board from an existing grid.
    /// </summary>
    /// <param name="grid">The grid to wrap.</param>
    public Board(Grid<ICell> grid)
        : this(grid.ToListOfList()) { }

    /// <summary>
    /// Renders the board as an image.
    /// </summary>
    /// <returns>The image of the board.</returns>
    public WorldImage Render()
    {
        WorldImage emptyImage = new EmptyImage();

        var image = _grid.Fold(
            emptyImage,
            (image, row, index) =>
                new AboveImage(
                    row.Fold(
                        emptyImage,
                        (image, cell, index) => new BesideImage(cell.GetImage(), image)
                    ),
                    image
                )
        );

        return image;
    }
}

