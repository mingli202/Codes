namespace TreeFunctionPractice;

public abstract record NumTT;
public record NumLeaf(int val) : NumTT;
public record NumNode(int val, NumTT left, NumTT mid, NumTT right) : NumTT;

public abstract record ListOfInt;
public record EmptyLoInt() : ListOfInt;
public record LinkLoInt(int first, ListOfInt rest) : ListOfInt;
