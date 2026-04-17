namespace XML;

public interface IXML
{
    /// <summary>
    /// Determines if this XML document and the given document have exactly the same shape: 
    /// the same tags, the same tree-structure, the same attributes in the same order with 
    /// the same values, and the same content.
    /// </summary>
    /// <param name="other">The other document for comparison</param>
    bool SameDocument(IXML other);
    /// <summary>
    /// Determines if this XML document and the given document have equivalent shape, 
    /// when attribute-ordering is irrelevant (i.e., considered as a set rather than a list)
    /// </summary>
    /// <param name="other">The other document for comparison</param>
    bool SameXML(IXML other);
    /// <summary>
    /// Determines if this XML document and the given document contain the same plain text content,
    /// by ignoring all the tag structure and focusing just on the text.
    /// </summary>
    /// <param name="other">The other document for comparison</param>
    bool SameText(IXML other);

    /// <summary>
    /// Check if this IXML document and the given text are equal
    /// </summary>
    /// <param name="text">The text to compare with</param>
    bool SameDocumentText(Text text);

    /// <summary>
    /// Check if this IXML document and the given tag are equal
    /// </summary>
    /// <param name="tag">The tag to compare with</param>
    bool SameDocumentTag(Tag tag);

    /// <summary>
    /// Check if this IXML document and the given text are equivalent
    /// </summary>
    /// <param name="text">The text to compare with</param>
    bool SameXMLText(Text text);

    /// <summary>
    /// Check if this IXML document and the given tag are equivalent
    /// </summary>
    /// <param name="tag">The tag to compare with</param>
    bool SameXMLTag(Tag tag);

    /// <summary>
    /// Get the plain text of this XML document
    /// </summary>
    string GetPlainText();
}

