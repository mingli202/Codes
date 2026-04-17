using System.Windows.Media;
using ImageLib;
using ImageLib.Enumerations;

namespace Automata;

// You may enhance the interfaces with new methods if necessary,
// but you may not edit these existing methods.
public interface ICell : IOperation
{
    // gets the state of this ICell
    int GetState();

    // render this ICell as an image of a rectangle with this width and height
    WorldImage Render(int width, int height);

    // produces the child cell of this ICell with the given left and right neighbors
    ICell ChildCell(ICell left, ICell right);
}

public abstract class CellBase(int state) : ICell
{
    public int GetState() => state;

    public WorldImage Render(int width, int height) =>
        new RectangleImage(width, height, OutlineMode.Fill, state == 0 ? Colors.White : Colors.Black);

    public abstract ICell ChildCell(ICell left, ICell right);

    public static IOperation Or(IOperation opA, IOperation opB) => Operation.Or(opA, opB);

    public static IOperation And(IOperation opA, IOperation opB) => Operation.And(opA, opB);

    public static IOperation Xor(IOperation opA, IOperation opB) => Operation.Xor(opA, opB);

    public static IOperation Not(IOperation opA) => Operation.Not(opA);
}

public class InertCell() : CellBase(0)
{
    public override ICell ChildCell(ICell left, ICell right) => this;
}

public class Rule60(int state) : CellBase(state)
{
    public override ICell ChildCell(ICell left, ICell right) =>
        new Rule60(Xor(left, this).GetState());
}

public class Rule30(int state) : CellBase(state)
{
    public override ICell ChildCell(ICell left, ICell right) =>
        new Rule30(
            Or(And(left, Not(Or(this, right))), And(Not(left), Or(this, right))).GetState()
        );
}

