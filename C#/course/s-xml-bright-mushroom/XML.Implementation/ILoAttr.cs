namespace XML;

public interface ILoAttr
{
    bool Equals(ILoAttr other);
    bool EqualsLink(LinkLoAttr other);
    bool EqualsEmpty(EmptyLoAttr other);

    bool Equivalent(ILoAttr other);
    bool EquivalentLink(LinkLoAttr other);
    bool EquivalentEmpty(EmptyLoAttr other);

    bool ContainsDuplicates();
    bool Contains(Attr attr);
    bool All(Func<Attr, bool> predicate);
    bool Any(Func<Attr, bool> predicate);
}
