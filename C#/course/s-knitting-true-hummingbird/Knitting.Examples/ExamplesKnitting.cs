using TesterLib;

namespace Knitting;

using InstructionEnumerator = EnumerableEnumerator<IInstruction>;
using StitchEnumerator = EnumerableEnumerator<IStitch>;

#pragma warning disable CA1822, IDE0051
class ExamplesKnitting
{
    /// <summary>
    /// Verifies the public stitch rendering and flip behavior.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when knit and purl stitches render and flip as expected.</returns>
    bool TestStitchRenderingAndFlip(Tester t)
    {
        var knit = new Knit();
        var purl = new Purl();

        return t.CheckExpect(knit.ToString(), "V")
            && t.CheckExpect(purl.ToString(), "-")
            && t.CheckExpect(knit.Flip() is Purl, true)
            && t.CheckExpect(purl.Flip() is Knit, true);
    }

    /// <summary>
    /// Verifies the stitch double-dispatch equality helpers.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when stitch comparisons distinguish knit and purl correctly.</returns>
    bool TestStitchEquality(Tester t)
    {
        var knit = new Knit();
        var otherKnit = new Knit();
        var purl = new Purl();

        return t.CheckExpect(knit.Equals(otherKnit), true)
            && t.CheckExpect(knit.Equals(purl), false)
            && t.CheckExpect(purl.Equals(knit), false)
            && t.CheckExpect(purl.Equals(new Purl()), true)
            && t.CheckExpect(knit.EqualsKnit(otherKnit), true)
            && t.CheckExpect(knit.EqualsPurl(purl), false)
            && t.CheckExpect(purl.EqualsKnit(knit), false)
            && t.CheckExpect(purl.EqualsPurl(new Purl()), true);
    }

    /// <summary>
    /// Verifies that empty fabrics and empty instruction sets render as empty strings.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when empty states produce empty fabric renderings.</returns>
    bool TestEmptyFabricAndInstructions(Tester t)
    {
        var fabric = new KnittedFabric();
        var instructions = new KnitFabricInstructions();

        return t.CheckExpect(fabric.RenderFabric(), "")
            && t.CheckExpect(instructions.MakeFabric().RenderFabric(), "");
    }

    /// <summary>
    /// Verifies that <see cref="IEnumerableEnumerator{T}.Add(T)"/> appends stitches in order.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the appended sequence renders as expected.</returns>
    bool TestEnumerableEnumeratorAdd(Tester t)
    {
        IEnumerableEnumerator<IStitch> enumerable = StitchEnumerator.From([]);
        enumerable = enumerable.Add(new Knit());
        enumerable = enumerable.Add(new Purl());

        return t.CheckExpect(enumerable.ToString(), "V-");
    }

    /// <summary>
    /// Verifies that a single fabric row renders correctly.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the rendered row matches the expected pattern.</returns>
    bool TestFabric1(Tester t)
    {
        var fabric = new KnittedFabric();

        fabric = fabric.AddRow(
            StitchEnumerator.From([
                new Knit(),
                new Knit(),
                new Knit(),
                new Purl(),
                new Purl(),
                new Purl(),
                new Knit(),
            ])
        );

        var rendered = fabric.RenderFabric();

        return t.CheckExpect(rendered, "VVV---V");
    }

    /// <summary>
    /// Verifies that multiple fabric rows render from top to bottom.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the rendered fabric matches the expected layout.</returns>
    bool TestFabric2(Tester t)
    {
        var fabric = new KnittedFabric();

        fabric = fabric.AddRow(
            StitchEnumerator.From([
                new Knit(),
                new Knit(),
                new Knit(),
                new Purl(),
                new Purl(),
                new Purl(),
                new Knit(),
            ])
        );

        fabric = fabric.AddRow(
            StitchEnumerator.From([
                new Knit(),
                new Purl(),
                new Purl(),
                new Purl(),
                new Knit(),
                new Knit(),
                new Knit(),
            ])
        );

        var rendered = fabric.RenderFabric();

        return t.CheckExpect(rendered, "V---VVV\nVVV---V");
    }

    /// <summary>
    /// Verifies that both instruction types expand into the expected stitch sequences.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when knit and purl instructions produce the right stitches.</returns>
    bool TestInstructionTypes(Tester t)
    {
        var knitInstruction = new KnitInstruction(3);
        var purlInstruction = new PurlInstruction(2);

        return t.CheckExpect(knitInstruction.GetStitches().ToString(), "VVV")
            && t.CheckExpect(purlInstruction.GetStitches().ToString(), "--");
    }

    /// <summary>
    /// Verifies that enumerators created from enumerable sources expose manual iteration members correctly.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when enumeration, current item access, and reset all behave as expected.</returns>
    bool TestEnumerableEnumeratorManualIteration(Tester t)
    {
        var enumerable = StitchEnumerator.From([new Knit(), new Purl()]);
        var enumerator = enumerable.GetEnumerator();
        var genericEnumerator = ((IEnumerable<IStitch>)enumerable).GetEnumerator();
        var nongenericEnumerator = ((System.Collections.IEnumerable)enumerable).GetEnumerator();

        var firstMove = enumerator.MoveNext();
        var firstCurrent = enumerator.Current.ToString();
        var secondMove = enumerator.MoveNext();
        var secondCurrent = enumerator.Current.ToString();

        enumerator.Reset();
        var resetMove = enumerator.MoveNext();
        var resetCurrent = enumerator.Current.ToString();

        genericEnumerator.MoveNext();
        nongenericEnumerator.MoveNext();

        return t.CheckExpect(firstMove, true)
            && t.CheckExpect(firstCurrent, "V")
            && t.CheckExpect(secondMove, true)
            && t.CheckExpect(secondCurrent, "-")
            && t.CheckExpect(resetMove, true)
            && t.CheckExpect(resetCurrent, "V")
            && t.CheckExpect(genericEnumerator.Current.ToString(), "V")
            && t.CheckExpect(nongenericEnumerator.Current is Knit, true);
    }

    /// <summary>
    /// Verifies that enumerators created from enumerator sources consume only the remaining items.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the wrapped enumerator yields the unconsumed suffix.</returns>
    bool TestEnumerableEnumeratorFromEnumerator(Tester t)
    {
        var source = StitchEnumerator.From([new Knit(), new Purl(), new Knit()]).GetEnumerator();
        source.MoveNext();

        var wrapped = StitchEnumerator.From(source);

        return t.CheckExpect(wrapped.ToString(), "-V");
    }

    /// <summary>
    /// Verifies that repeating a stitch sequence duplicates the full sequence in order.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the repeated sequence renders as expected.</returns>
    bool TestEnumerableEnumeratorRepeat(Tester t)
    {
        IEnumerableEnumerator<IStitch> enumerable = StitchEnumerator.From([new Knit(), new Purl()]);
        enumerable = enumerable.Repeat(2);

        return t.CheckExpect(enumerable.ToString(), "V-V-");
    }

    /// <summary>
    /// Verifies that repeating instruction sequences expands into the expected stitches.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the repeated instructions produce the expected fabric row.</returns>
    bool TestEnumerableEnumeratorRepeat2(Tester t)
    {
        IEnumerator<IInstruction> instructionsEnum = InstructionEnumerator.From([
            new KnitInstruction(2),
            new PurlInstruction(3),
        ]);

        IEnumerableEnumerator<IInstruction> instructions = InstructionEnumerator.From(
            instructionsEnum
        );

        instructions = instructions.Repeat(2);

        var fabric = StitchEnumerator.From(instructions.SelectMany(i => i.GetStitches()));

        return t.CheckExpect(fabric.ToString(), "VV---VV---");
    }

    /// <summary>
    /// Verifies that folding a stitch sequence accumulates a derived result.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the fold result matches the expected aggregate.</returns>
    bool TestEnumerableEnumeratorFold(Tester t)
    {
        var enumerable = StitchEnumerator.From([new Knit(), new Purl(), new Knit()]);

        var score = enumerable.Fold(0, (acc, stitch) => acc + (stitch is Knit ? 2 : 1));

        return t.CheckExpect(score, 5);
    }

    /// <summary>
    /// Verifies that disposing an enumerator is safe for list-backed sequences.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when disposing does not throw.</returns>
    bool TestEnumerableEnumeratorDispose(Tester t)
    {
        var enumerator = StitchEnumerator.From([new Knit()]).GetEnumerator();
        var disposedWithoutThrowing = true;

        try
        {
            enumerator.Dispose();
        }
        catch
        {
            disposedWithoutThrowing = false;
        }

        return t.CheckExpect(disposedWithoutThrowing, true);
    }

    /// <summary>
    /// Verifies that a single row of instructions builds the expected fabric.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the generated fabric matches the expected rendering.</returns>
    bool TestInstructions(Tester t)
    {
        var instructions = new KnitFabricInstructions();
        instructions = instructions.AddRow(
            InstructionEnumerator.From([
                new KnitInstruction(2),
                new PurlInstruction(3),
                new RepeatInstruction(3, InstructionEnumerator.From([new KnitInstruction(2)])),
            ])
        );

        var fabric = instructions.MakeFabric();

        return t.CheckExpect(fabric.RenderFabric(), "VVVVVV---VV");
    }

    /// <summary>
    /// Verifies that a repeat instruction expands its nested instructions correctly.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the repeated instruction yields the expected stitches.</returns>
    bool TestRepeatInstruction(Tester t)
    {
        var instruction = new RepeatInstruction(
            3,
            InstructionEnumerator.From([new KnitInstruction(2)])
        );
        var fabric = instruction.GetStitches();

        return t.CheckExpect(fabric.ToString(), "VVVVVV");
    }

    /// <summary>
    /// Verifies that multiple instruction rows account for alternating knitting direction.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the produced fabric matches the expected rendering.</returns>
    bool TestInstructions2(Tester t)
    {
        var instructions = new KnitFabricInstructions();
        instructions = instructions
            .AddRow(
                InstructionEnumerator.From([
                    new KnitInstruction(2),
                    new PurlInstruction(3),
                    new RepeatInstruction(2, InstructionEnumerator.From([new KnitInstruction(2)])),
                ])
            )
            .AddRow(
                InstructionEnumerator.From([
                    new KnitInstruction(1),
                    new PurlInstruction(2),
                    new RepeatInstruction(
                        2,
                        InstructionEnumerator.From([new KnitInstruction(2), new PurlInstruction(3)])
                    ),
                ])
            );

        var fabric = instructions.MakeFabric();

        return t.CheckExpect(
            fabric.RenderFabric(),
            string.Join('\n', ["-VV--VVV--VVV", "VVVV---VV"])
        );
    }

    /// <summary>
    /// Verifies that instruction equality compares the fabrics produced by two instruction sets.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when matching instructions compare equal and different instructions do not.</returns>
    bool TestSameInstructions(Tester t)
    {
        var instructions1 = new KnitFabricInstructions().AddRow(
            InstructionEnumerator.From([new KnitInstruction(2), new PurlInstruction(1)])
        );

        var instructions2 = new KnitFabricInstructions().AddRow(
            InstructionEnumerator.From([new KnitInstruction(2), new PurlInstruction(1)])
        );

        var instructions3 = new KnitFabricInstructions().AddRow(
            InstructionEnumerator.From([new PurlInstruction(2), new KnitInstruction(1)])
        );

        return t.CheckExpect(instructions1.SameInstructions(instructions2), true)
            && t.CheckExpect(instructions1.SameInstructions(instructions3), false);
    }

    /// <summary>
    /// Verifies that cloning preserves the sequence for both the original and the clone.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when both sequences render identically after cloning.</returns>
    bool TestEnumerableEnumeratorClone(Tester t)
    {
        IEnumerableEnumerator<IStitch> enumerable = StitchEnumerator.From(
            StitchEnumerator.From([new Knit(), new Purl()]).GetEnumerator()
        );
        var clone = enumerable.Clone();
        var enumerableString = enumerable.ToString();
        var cloneString = clone.ToString();

        return t.CheckExpect(cloneString, "V-") && t.CheckExpect(enumerableString.ToString(), "V-");
    }

    /// <summary>
    /// Verifies that cloning a fabric produces an independent copy with the same rendering.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the clone matches the original and later changes do not affect the source fabric.</returns>
    bool TestKnittedFabricClone(Tester t)
    {
        var original = new KnittedFabric().AddRow(
            StitchEnumerator.From([new Knit(), new Purl(), new Knit()])
        );

        var clone = original.Clone();
        var mutatedClone = clone.AddRow(StitchEnumerator.From([new Purl(), new Purl(), new Knit()]));

        return t.CheckExpect(clone.RenderFabric(), "V-V")
            && t.CheckExpect(original.RenderFabric(), "V-V")
            && t.CheckExpect(mutatedClone.RenderFabric(), "--V\nV-V")
            && t.CheckExpect(original.RenderFabric(), "V-V");
    }

    /// <summary>
    /// Verifies that flipping a fabric reverses rows and swaps knit and purl stitches.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when both the original and flipped renderings match expectations.</returns>
    bool TestFlipFabric(Tester t)
    {
        var fabric = new KnittedFabric()
            .AddRow(
                StitchEnumerator.From([new Knit(), new Knit(), new Purl(), new Purl(), new Knit()])
            )
            .AddRow(
                StitchEnumerator.From([new Purl(), new Knit(), new Purl(), new Knit(), new Knit()])
            );

        return t.CheckExpect(fabric.RenderFabric(), string.Join("\n", ["-V-VV", "VV--V"]))
            && t.CheckExpect(fabric.Flip().RenderFabric(), string.Join("\n", ["--V-V", "-VV--"]));
    }

    /// <summary>
    /// Verifies that the fabric helper renderer matches the public fabric rendering.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when both rendering paths produce the same output.</returns>
    bool TestRenderFabricHelper(Tester t)
    {
        var fabric = new KnittedFabric()
            .AddRow(StitchEnumerator.From([new Knit(), new Purl()]))
            .AddRow(StitchEnumerator.From([new Purl(), new Knit()]));

        return t.CheckExpect(fabric.Clone().RenderFabricHelper(), fabric.RenderFabric());
    }

    /// <summary>
    /// Verifies that fabric equality accepts flipped orientation but not different row order.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when the expected equality comparisons hold.</returns>
    bool TestSameFabric(Tester t)
    {
        var fabric1 = new KnittedFabric()
            .AddRow(
                StitchEnumerator.From([new Knit(), new Knit(), new Purl(), new Purl(), new Knit()])
            )
            .AddRow(
                StitchEnumerator.From([new Purl(), new Knit(), new Purl(), new Knit(), new Knit()])
            );

        var fabric2 = new KnittedFabric()
            .AddRow(
                StitchEnumerator.From([new Purl(), new Knit(), new Knit(), new Purl(), new Purl()])
            )
            .AddRow(
                StitchEnumerator.From([new Purl(), new Purl(), new Knit(), new Purl(), new Knit()])
            );

        var fabric3 = new KnittedFabric()
            .AddRow(
                StitchEnumerator.From([new Purl(), new Purl(), new Knit(), new Purl(), new Knit()])
            )
            .AddRow(
                StitchEnumerator.From([new Purl(), new Knit(), new Knit(), new Purl(), new Purl()])
            );

        return t.CheckExpect(fabric1.SameFabric(fabric2), true)
            && t.CheckExpect(fabric1.SameFabric(fabric3), false)
            && t.CheckExpect(fabric2.SameFabric(fabric3), false);
    }

    /// <summary>
    /// Verifies that fabric equality rejects fabrics with different dimensions.
    /// </summary>
    /// <param name="t">The tester used to record expectations.</param>
    /// <returns><see langword="true"/> when mismatched row counts and row widths compare as different fabrics.</returns>
    bool TestSameFabricDifferentSizes(Tester t)
    {
        var baseFabric = new KnittedFabric().AddRow(
            StitchEnumerator.From([new Knit(), new Purl(), new Knit()])
        );

        var extraRowFabric = baseFabric.AddRow(
            StitchEnumerator.From([new Purl(), new Knit(), new Purl()])
        );

        var widerRowFabric = new KnittedFabric().AddRow(
            StitchEnumerator.From([new Knit(), new Purl(), new Knit(), new Purl()])
        );

        return t.CheckExpect(baseFabric.SameFabric(extraRowFabric), false)
            && t.CheckExpect(baseFabric.SameFabric(widerRowFabric), false);
    }
}

