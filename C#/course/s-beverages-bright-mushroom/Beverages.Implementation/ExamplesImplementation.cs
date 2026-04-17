using TesterLib;


namespace Beverages;

class ExamplesImplementation
{
    bool TestContains(Tester t)
    {
        var list = new LinkLoString("a", new LinkLoString("b", new LinkLoString("c", new EmptyLoString())));
        return t.CheckExpect(list.Contains("a"), true) && t.CheckExpect(list.Contains("b"), true) && t.CheckExpect(list.Contains("c"), true) && t.CheckExpect(list.Contains("d"), false);
    }

    bool TestFormat(Tester t)
    {
        var list = new LinkLoString("a", new LinkLoString("b", new LinkLoString("c", new EmptyLoString())));
        return t.CheckExpect(list.Format(), "a, b, c");
    }

    bool TestFormatStartingWithComma(Tester t)
    {
        var list = new LinkLoString("a", new LinkLoString("b", new LinkLoString("c", new EmptyLoString())));
        return t.CheckExpect(list.FormatStartingWithComma(), ", a, b, c");
    }
}

