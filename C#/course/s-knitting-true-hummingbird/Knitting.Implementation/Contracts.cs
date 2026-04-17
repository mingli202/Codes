namespace Knitting;

// You may enhance the interfaces with new methods if necessary,
// but you may not edit these existing methods.
//
using InstructionEnumerator = EnumerableEnumerator<IInstruction>;
using StitchEnumerator = EnumerableEnumerator<IStitch>;

public interface IStitch
{
    /// <summary>
    /// Creates the opposite stitch representation.
    /// </summary>
    /// <returns>A stitch with the opposite knit or purl orientation.</returns>
    IStitch Flip();

    /// <summary>
    /// Determines whether this stitch matches another stitch.
    /// </summary>
    /// <param name="other">The stitch to compare against.</param>
    /// <returns><see langword="true"/> when both stitches have the same kind; otherwise, <see langword="false"/>.</returns>
    bool Equals(IStitch other);

    /// <summary>
    /// Determines whether this stitch matches a knit stitch.
    /// </summary>
    /// <param name="other">The knit stitch to compare against.</param>
    /// <returns><see langword="true"/> when this stitch is knit; otherwise, <see langword="false"/>.</returns>
    bool EqualsKnit(Knit other);

    /// <summary>
    /// Determines whether this stitch matches a purl stitch.
    /// </summary>
    /// <param name="other">The purl stitch to compare against.</param>
    /// <returns><see langword="true"/> when this stitch is purl; otherwise, <see langword="false"/>.</returns>
    bool EqualsPurl(Purl other);
}

public class Knit : IStitch
{
    /// <summary>
    /// Renders a knit stitch using the project's visual notation.
    /// </summary>
    /// <returns>The character representation for a knit stitch.</returns>
    public override string ToString() => "V";

    /// <summary>
    /// Converts this knit stitch into its opposite stitch.
    /// </summary>
    /// <returns>A new <see cref="Purl"/> stitch.</returns>
    public IStitch Flip() => new Purl();

    /// <summary>
    /// Determines whether another stitch is also knit.
    /// </summary>
    /// <param name="other">The stitch to compare against.</param>
    /// <returns><see langword="true"/> when <paramref name="other"/> is knit; otherwise, <see langword="false"/>.</returns>
    public bool Equals(IStitch other) => other.EqualsKnit(this);

    /// <summary>
    /// Determines whether another knit stitch matches this one.
    /// </summary>
    /// <param name="other">The knit stitch to compare against.</param>
    /// <returns>Always <see langword="true"/> for knit-to-knit comparisons.</returns>
    public bool EqualsKnit(Knit other) => true;

    /// <summary>
    /// Determines whether a purl stitch matches this knit stitch.
    /// </summary>
    /// <param name="other">The purl stitch to compare against.</param>
    /// <returns>Always <see langword="false"/> for knit-to-purl comparisons.</returns>
    public bool EqualsPurl(Purl other) => false;
}

public class Purl : IStitch
{
    /// <summary>
    /// Renders a purl stitch using the project's visual notation.
    /// </summary>
    /// <returns>The character representation for a purl stitch.</returns>
    public override string ToString() => "-";

    /// <summary>
    /// Converts this purl stitch into its opposite stitch.
    /// </summary>
    /// <returns>A new <see cref="Knit"/> stitch.</returns>
    public IStitch Flip() => new Knit();

    /// <summary>
    /// Determines whether another stitch is also purl.
    /// </summary>
    /// <param name="other">The stitch to compare against.</param>
    /// <returns><see langword="true"/> when <paramref name="other"/> is purl; otherwise, <see langword="false"/>.</returns>
    public bool Equals(IStitch other) => other.EqualsPurl(this);

    /// <summary>
    /// Determines whether a knit stitch matches this purl stitch.
    /// </summary>
    /// <param name="other">The knit stitch to compare against.</param>
    /// <returns>Always <see langword="false"/> for purl-to-knit comparisons.</returns>
    public bool EqualsKnit(Knit other) => false;

    /// <summary>
    /// Determines whether another purl stitch matches this one.
    /// </summary>
    /// <param name="other">The purl stitch to compare against.</param>
    /// <returns>Always <see langword="true"/> for purl-to-purl comparisons.</returns>
    public bool EqualsPurl(Purl other) => true;
}

public interface IInstruction
{
    /// <summary>
    /// Expands an instruction into the stitches it produces.
    /// </summary>
    /// <returns>An enumerator over the generated stitches.</returns>
    IEnumerableEnumerator<IStitch> GetStitches();
}

public abstract class Instruction(int numberOfRepeats) : IInstruction
{
    protected readonly int numberOfRepeats = numberOfRepeats;

    /// <summary>
    /// Expands the instruction into the stitches it produces.
    /// </summary>
    /// <returns>An enumerator over the generated stitches.</returns>
    public abstract IEnumerableEnumerator<IStitch> GetStitches();
}

public class KnitInstruction(int numberOfRepeats) : Instruction(numberOfRepeats)
{
    /// <summary>
    /// Produces the requested number of knit stitches.
    /// </summary>
    /// <returns>An enumerator containing <c>numberOfRepeats</c> knit stitches.</returns>
    public override IEnumerableEnumerator<IStitch> GetStitches() =>
        StitchEnumerator.From(Enumerable.Repeat(new Knit(), numberOfRepeats));
}

public class PurlInstruction(int numberOfRepeats) : Instruction(numberOfRepeats)
{
    /// <summary>
    /// Produces the requested number of purl stitches.
    /// </summary>
    /// <returns>An enumerator containing <c>numberOfRepeats</c> purl stitches.</returns>
    public override IEnumerableEnumerator<IStitch> GetStitches() =>
        StitchEnumerator.From(Enumerable.Repeat(new Purl(), numberOfRepeats));
}

public class RepeatInstruction(int numberOfRepeats, IEnumerator<IInstruction> instructions)
    : Instruction(numberOfRepeats)
{
    /// <summary>
    /// Repeats the supplied instruction sequence and flattens it into stitches.
    /// </summary>
    /// <returns>An enumerator containing the repeated instruction output.</returns>
    public override IEnumerableEnumerator<IStitch> GetStitches() =>
        StitchEnumerator.From(
            InstructionEnumerator
                .From(instructions)
                .Repeat(numberOfRepeats)
                .SelectMany(instruction => instruction.GetStitches())
        );
}
