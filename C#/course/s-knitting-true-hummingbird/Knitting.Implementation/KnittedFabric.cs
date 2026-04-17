namespace Knitting;

using StitchEnumerator = EnumerableEnumerator<IStitch>;

// MAKE SURE TO GIVE SUFFICIENT INTERPRETATIONS
// FOR ANY CLASSES YOU CREATE IN YOUR DESIGN!

// MAKE SURE TO GIVE SUFFICIENT DOCUMENTATION,
// INCLUDING EFFECT STATEMENTS, FOR ALL METHODS
// IN YOUR DESIGN!

public enum Direction
{
    Front,
    Back,
}

public class KnittedFabric
{
    private readonly IEnumerableEnumerator<IEnumerableEnumerator<IStitch>> rows;

    /// <summary>
    /// Creates an empty knitted fabric with no rows.
    /// </summary>
    public KnittedFabric()
    {
        rows = EnumerableEnumerator<IEnumerableEnumerator<IStitch>>.From([]);
    }

    /// <summary>
    /// Creates a knitted fabric backed by the provided row sequence.
    /// </summary>
    /// <param name="rows">The rows that make up the fabric.</param>
    private KnittedFabric(IEnumerableEnumerator<IEnumerableEnumerator<IStitch>> rows)
    {
        this.rows = rows;
    }

    /// <summary>
    /// Returns a new fabric with an additional row appended.
    /// </summary>
    /// <param name="rowIter">The stitches that make up the new row.</param>
    /// <returns>A new fabric containing the added row.</returns>
    public KnittedFabric AddRow(IEnumerator<IStitch> rowIter) =>
        new(rows.Add(StitchEnumerator.From(rowIter)));

    /// <summary>
    /// Renders the fabric from top row to bottom row using stitch notation.
    /// </summary>
    /// <returns>A multiline string representation of the fabric.</returns>
    public string RenderFabric() => Clone().RenderFabricHelper();

    /// <summary>
    /// Renders the current row storage order without cloning again.
    /// </summary>
    /// <returns>A multiline string representation of the stored rows.</returns>
    public string RenderFabricHelper() =>
        string.Join("\n", rows.Select(row => string.Join("", row.ToString())).Reverse());


    /// <summary>
    /// Determines whether two fabrics represent the same final fabric, allowing for flipped orientation.
    /// </summary>
    /// <param name="fabric">The fabric to compare against.</param>
    /// <returns><see langword="true"/> when the fabrics match directly or after flipping; otherwise, <see langword="false"/>.</returns>
    public bool SameFabric(KnittedFabric fabric) => Clone().SameFabricHelper(fabric.Clone()) || Flip().SameFabricHelper(fabric.Clone());

    /// <summary>
    /// Compares this fabric with another fabric row by row and stitch by stitch.
    /// </summary>
    /// <param name="fabric">The fabric to compare against.</param>
    /// <returns><see langword="true"/> when all paired rows and stitches are equal; otherwise, <see langword="false"/>.</returns>
    private bool SameFabricHelper(KnittedFabric fabric)
    {
        var leftRows = rows.Clone().Select(row => row.Clone().ToList()).ToList();
        var rightRows = fabric.rows.Clone().Select(row => row.Clone().ToList()).ToList();

        return leftRows.Count == rightRows.Count
            && leftRows.Zip(
                    rightRows,
                    (rowA, rowB) =>
                        rowA.Count == rowB.Count
                        && rowA.Zip(rowB, (stitchA, stitchB) => stitchA.Equals(stitchB)).All(x => x)
                )
                .All(x => x);
    }

    /// <summary>
    /// Produces a flipped view of the fabric by reversing each row and flipping each stitch.
    /// </summary>
    /// <returns>A new fabric representing the opposite side of the same knitting.</returns>
    public KnittedFabric Flip()
    {
        var fabric = new KnittedFabric();
        var clone = Clone();
        foreach (var row in clone.rows)
        {
            fabric = fabric.AddRow(row.Select(stitch => stitch.Flip()).Reverse().GetEnumerator());
        }
        return fabric;
    }

    /// <summary>
    /// Creates a deep copy of the fabric rows so the result can be consumed independently.
    /// </summary>
    /// <returns>A new fabric containing clones of each row.</returns>
    public KnittedFabric Clone()
    {
        var fabric = new KnittedFabric();
        var clonedRows = rows.Clone().Select(row => row.Clone());
        foreach (var row in clonedRows)
        {
            fabric = fabric.AddRow(row);
        }
        return fabric;
    }
}

public class KnitFabricInstructions
{
    private readonly Direction direction;
    private readonly KnittedFabric fabric;

    /// <summary>
    /// Creates an empty instruction sequence positioned to add the first row from the back side.
    /// </summary>
    public KnitFabricInstructions()
    {
        direction = Direction.Back;
        fabric = new KnittedFabric();
    }

    /// <summary>
    /// Creates an instruction state with a known direction and accumulated fabric.
    /// </summary>
    /// <param name="direction">The side from which the next row will be interpreted.</param>
    /// <param name="fabric">The fabric produced so far.</param>
    private KnitFabricInstructions(Direction direction, KnittedFabric fabric)
    {
        this.direction = direction;
        this.fabric = fabric;
    }

    /// <summary>
    /// Adds a row of instructions and updates the fabric according to the next knitting direction.
    /// </summary>
    /// <param name="instructions">The instructions that define the next row.</param>
    /// <returns>A new instruction state containing the updated fabric and direction.</returns>
    public KnitFabricInstructions AddRow(IEnumerator<IInstruction> instructions)
    {
        var stiches = EnumerableEnumerator<IInstruction>
            .From(instructions)
            .SelectMany(i => i.GetStitches())
            .Reverse();

        var nextDirection = NextDirection();

        if (nextDirection == Direction.Back)
        {
            stiches = stiches.Select(i => i.Flip()).Reverse();
        }

        var newFabric = fabric.AddRow(stiches.GetEnumerator());

        return new(nextDirection, newFabric);
    }

    /// <summary>
    /// Determines whether two instruction sets produce equivalent fabrics.
    /// </summary>
    /// <param name="other">The other instruction set to compare against.</param>
    /// <returns><see langword="true"/> when both instruction sets produce the same fabric; otherwise, <see langword="false"/>.</returns>
    public bool SameInstructions(KnitFabricInstructions other) => fabric.SameFabric(other.fabric);

    /// <summary>
    /// Returns the fabric produced by the accumulated instructions.
    /// </summary>
    /// <returns>The knitted fabric built so far.</returns>
    public KnittedFabric MakeFabric() => fabric;

    /// <summary>
    /// Computes the direction that should be used for the next added row.
    /// </summary>
    /// <returns>The opposite of the current direction.</returns>
    private Direction NextDirection() =>
        direction == Direction.Front ? Direction.Back : Direction.Front;
}
