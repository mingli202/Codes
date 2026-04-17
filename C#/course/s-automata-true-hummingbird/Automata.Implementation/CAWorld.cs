using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;
using ImageLib.ImpWorld;

namespace Automata;

public class CAWorld : World
{
    // constants
    const int CellWidth = 10;
    const int CellHeight = 10;
    const int InitialOffCells = 20;
    const int TotalCells = InitialOffCells * 2 + 1;
    const int NumHistory = 41;
    public const int TotalWidth = TotalCells * CellWidth;
    public const int TotalHeight = NumHistory * CellHeight;

    // the current generation of cells
    private CellArray curGen = [];

    // the history of previous generations (earliest state at the start of the list)
    private List<CellArray> history = [];

    // Constructs a CAWorld with InitialOffCells of off cells on the left,
    // then one on cell, then InitialOffCells of off cells on the right
    public CAWorld(ICell off, ICell on)
    {
        for (int i = 0; i < InitialOffCells; i++)
        {
            curGen = [.. curGen, off];
        }

        curGen = [.. curGen, on];

        for (int i = 0; i < InitialOffCells; i++)
        {
            curGen = [.. curGen, off];
        }

        if (curGen.Count != TotalCells)
        {
            throw new Exception("The number of cells in the initial generation is incorrect");
        }
    }

    // Modifies this CAWorld by adding the current generation to the history
    // and setting the current generation to the next one
    protected override void OnTick()
    {
        history = [.. history.Append(curGen).TakeLast(NumHistory)];
        curGen = curGen.NextGen();
    }

    // Draws the current world, ``scrolling up'' from the bottom of the image
    protected WorldImage MakeImage()
    {
        // make a light-gray background image big enough to hold 41 generations of 41 cells each
        WorldImage bg = new RectangleImage(
            TotalWidth,
            TotalHeight,
            OutlineMode.Fill,
            Color.FromRgb(240, 240, 240)
        );

        // build up the image containing the past and current cells
        WorldImage cells = new EmptyImage();
        foreach (CellArray array in this.history)
        {
            cells = new AboveImage(cells, array.Draw(CellWidth, CellHeight));
        }
        cells = new AboveImage(cells, this.curGen.Draw(CellWidth, CellHeight));

        // draw all the cells onto the background
        return new OverlayOffsetAlignImage(AlignModeX.Center, AlignModeY.Bottom, cells, 0, 0, bg);
    }

    protected override WorldScene MakeScene()
    {
        WorldScene canvas = new WorldScene(TotalWidth, TotalHeight);
        canvas.PlaceImageXY(this.MakeImage(), TotalWidth / 2, TotalHeight / 2);
        return canvas;
    }
}

class CellArray : List<ICell>
{
    public CellArray NextGen() =>
        [
            .. this.Select(
                (cell, i) =>
                {
                    var left = i - 1 < 0 ? new InertCell() : this[i - 1];
                    var right = i + 1 >= Count ? new InertCell() : this[i + 1];
                    return cell.ChildCell(left, right);
                }
            ),
        ];

    public WorldImage Draw(int cellWidth, int cellHeight) =>
        this.Aggregate(
            (WorldImage)new EmptyImage(),
            (acc, cell) => new BesideImage(acc, cell.Render(cellWidth, cellHeight))
        );
}


