namespace NumListPractice;

public abstract record ListOfInt();
public record EmptyLoInt() : ListOfInt;
public record LinkLoInt(int first, ListOfInt rest) : ListOfInt;
public abstract record ListOfListOfInt();
public record EmptyLoLoInt() : ListOfListOfInt;
public record LinkLoLoInt(ListOfInt first, ListOfListOfInt rest) : ListOfListOfInt;

