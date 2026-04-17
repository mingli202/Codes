namespace XML;

public class LinkLoXML(IXML first, ILoXML rest) : ILoXML
{
    private readonly IXML _first = first;
    private readonly ILoXML _rest = rest;

    public bool Equals(ILoXML other) => other.EqualsLink(this);
    public bool EqualsLink(LinkLoXML other) => other._first.SameDocument(_first) && other._rest.Equals(_rest);
    public bool EqualsEmpty(EmptyLoXML other) => false;

    public bool Equivalent(ILoXML other) => other.EquivalentLink(this);
    public bool EquivalentLink(LinkLoXML other) => other._first.SameXML(_first) && other._rest.Equivalent(_rest);
    public bool EquivalentEmpty(EmptyLoXML other) => false;

    public string GetPlainText() => _first.GetPlainText() + _rest.GetPlainText();
}
