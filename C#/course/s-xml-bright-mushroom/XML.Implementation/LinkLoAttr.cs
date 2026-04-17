namespace XML;

public class LinkLoAttr(Attr first, ILoAttr rest) : ILoAttr
{
    private readonly Attr _first = first;
    private readonly ILoAttr _rest = rest;

    public bool Equals(ILoAttr other) => other.EqualsLink(this);
    public bool EqualsLink(LinkLoAttr other) => other._first.Equals(_first) && other._rest.Equals(_rest);
    public bool EqualsEmpty(EmptyLoAttr other) => false;

    public bool Equivalent(ILoAttr other) => other.EquivalentLink(this);
    public bool EquivalentLink(LinkLoAttr other) => other.All(Contains) && All(other.Contains);
    public bool EquivalentEmpty(EmptyLoAttr other) => false;

    public bool ContainsDuplicates() => _rest.Any(attr => attr.SameName(_first)) || _rest.ContainsDuplicates();

    public bool Contains(Attr attr) => Any((list_attr) => list_attr.Equals(attr));
    public bool All(Func<Attr, bool> predicate) => predicate(_first) && _rest.All(predicate);
    public bool Any(Func<Attr, bool> predicate) => predicate(_first) || _rest.Any(predicate);
}
