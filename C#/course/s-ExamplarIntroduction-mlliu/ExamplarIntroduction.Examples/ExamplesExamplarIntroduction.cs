namespace ExamplarIntroduction;

using TesterLib;

public class ExamplesExamplarIntroduction
{
    readonly ListOfInt someList = new LinkLoInt(1, new LinkLoInt(2, new LinkLoInt(3, new EmptyLoInt())));

    bool TestLength(Tester t) => t.CheckExpect(ListFunctions.Length(someList), 3);
    bool TestContains(Tester t) => t.CheckExpect(ListFunctions.Contains(someList, 1), true);
}

