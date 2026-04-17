namespace XML;

public class EmptyLoXML : ILoXML
{
    public bool Equals(ILoXML other) => other.EqualsEmpty(this);
    public bool EqualsLink(LinkLoXML other) => false;
    public bool EqualsEmpty(EmptyLoXML other) => true;

    public bool Equivalent(ILoXML other) => other.EquivalentEmpty(this);
    public bool EquivalentLink(LinkLoXML other) => false;
    public bool EquivalentEmpty(EmptyLoXML other) => true;

    public string GetPlainText() => string.Empty;
}
