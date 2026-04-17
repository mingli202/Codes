using TesterLib;

namespace Automata;

#pragma warning disable CA1822, IDE0051
class ExamplesAutomata
{
    bool TestOperationOr(Tester t)
    {
        var inputs = TwoInputs();
        var outputs = Map(inputs, Operation.Or);
        var expected = new List<int> { 0, 1, 1, 1 };

        return t.CheckExpect(outputs, expected);
    }

    bool TestOperationAnd(Tester t)
    {
        var inputs = TwoInputs();
        var outputs = Map(inputs, Operation.And);
        var expected = new List<int> { 0, 0, 0, 1 };

        return t.CheckExpect(outputs, expected);
    }

    bool TestOperationXor(Tester t)
    {
        var inputs = TwoInputs();
        var outputs = Map(inputs, Operation.Xor);
        var expected = new List<int> { 0, 1, 1, 0 };

        return t.CheckExpect(outputs, expected);
    }

    bool TestOperationNot(Tester t)
    {
        var opA = new Operation(0);
        var opB = new Operation(1);
        var outputs = new List<int>
        {
            Operation.Not(opA).GetState(),
            Operation.Not(opB).GetState(),
        };
        var expected = new List<int> { 1, 0 };

        return t.CheckExpect(outputs, expected);
    }

    bool TestRule60(Tester t)
    {
        List<(int left, int mid, int right, int expected)> inputs =
        [
            (1, 1, 1, 0),
            (1, 1, 0, 0),
            (1, 0, 1, 1),
            (1, 0, 0, 1),
            (0, 1, 1, 1),
            (0, 1, 0, 1),
            (0, 0, 1, 0),
            (0, 0, 0, 0),
        ];

        return CheckExpectRule(t, inputs, (i) => new Rule60(i));
    }

    bool TestRule30(Tester t)
    {
        List<(int left, int mid, int right, int expected)> inputs =
        [
            (1, 1, 1, 0),
            (1, 1, 0, 0),
            (1, 0, 1, 0),
            (1, 0, 0, 1),
            (0, 1, 1, 1),
            (0, 1, 0, 1),
            (0, 0, 1, 1),
            (0, 0, 0, 0),
        ];

        return CheckExpectRule(t, inputs, (i) => new Rule30(i));
    }

    bool TestCAWorldRule60(Tester t)
    {
        var off = new Rule60(0);
        var on = new Rule60(1);
        var world = new CAWorld(off, on);

        return t.CheckExpect(world.BigBang(410, 410, 1/30f), true);
    }

    bool TestCAWorldRule30(Tester t)
    {
        var off = new Rule30(0);
        var on = new Rule30(1);
        var world = new CAWorld(off, on);

        return t.CheckExpect(world.BigBang(410, 410, 1/30f), true);
    }

    private List<(int a, int b)> TwoInputs() => [(0, 0), (0, 1), (1, 0), (1, 1)];

    private List<int> Map(
        List<(int a, int b)> inputs,
        Func<IOperation, IOperation, IOperation> op
    ) =>
        [
            .. inputs.Select(
                (input) => op(new Operation(input.a), new Operation(input.b)).GetState()
            ),
        ];

    private bool CheckExpectRule(
        Tester t,
        List<(int left, int mid, int right, int expected)> inputs,
        Func<int, ICell> factory
    )
    {
        List<int> actual =
        [
            .. inputs.Select(
                (input) =>
                {
                    var left = factory(input.left);
                    var mid = factory(input.mid);
                    var right = factory(input.right);
                    return mid.ChildCell(left, right).GetState();
                }
            ),
        ];

        List<int> expected = [.. inputs.Select((input) => input.expected)];

        return t.CheckExpect(actual, expected);
    }
}


