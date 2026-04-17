namespace XML;

public class EmptyLoAttr : ILoAttr
{
    public bool Equals(ILoAttr other) => other.EqualsEmpty(this);
    public bool EqualsLink(LinkLoAttr other) => false;
    public bool EqualsEmpty(EmptyLoAttr other) => true;

    public bool Equivalent(ILoAttr other) => other.EquivalentEmpty(this);
    public bool EquivalentLink(LinkLoAttr other) => false;
    public bool EquivalentEmpty(EmptyLoAttr other) => true;

    public bool ContainsDuplicates() => false;
    public bool Contains(Attr attr) => false;
    public bool All(Func<Attr, bool> predicate) => true;
    public bool Any(Func<Attr, bool> predicate) => false;
}
