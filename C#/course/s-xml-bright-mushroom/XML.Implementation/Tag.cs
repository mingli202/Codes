namespace XML;

public class Tag : IXML
{
    readonly string _name;
    readonly ILoAttr _attrs;
    readonly ILoXML _content;

    public Tag(string name, ILoAttr attrs, ILoXML content)
    {
        if (attrs.ContainsDuplicates())
        {
            throw new ArgumentException("Duplicate attributes are not allowed");
        }

        _name = name;
        _attrs = attrs;
        _content = content;
    }

    public bool SameDocument(IXML doc) => doc.SameDocumentTag(this);

    public bool SameXML(IXML doc) => doc.SameXMLTag(this);

    public bool SameText(IXML doc) => doc.GetPlainText().Equals(GetPlainText());

    public bool SameDocumentText(Text text) => false;
    public bool SameDocumentTag(Tag tag) => tag._name.Equals(_name) && tag._attrs.Equals(_attrs) && tag._content.Equals(_content);

    public bool SameXMLText(Text text) => false;
    public bool SameXMLTag(Tag tag) => tag._name.Equals(_name) && tag._attrs.Equivalent(_attrs) && tag._content.Equivalent(_content);

    public string GetPlainText() => _content.GetPlainText();

}
