namespace XML;

public class Text(string content) : IXML
{
    private readonly string _content = content;

    public bool SameDocument(IXML doc) => doc.SameDocumentText(this);

    public bool SameXML(IXML doc) => doc.SameXMLText(this);

    public bool SameText(IXML doc) => doc.GetPlainText().Equals(GetPlainText());

    public bool SameDocumentText(Text text) => text._content.Equals(_content);
    public bool SameDocumentTag(Tag tag) => false;

    public bool SameXMLText(Text text) => text.SameDocument(this);
    public bool SameXMLTag(Tag tag) => false;

    public string GetPlainText() => _content;
}
