namespace XML;

public interface ILoXML
{
    /// <summary>
    /// Compares two XML objects for equality.
    /// </summary>
    /// <param name="other">The other XML object to compare with.</param>
    /// <returns>True if the two objects are equal, false otherwise.</returns>
    bool Equals(ILoXML other);

    /// <summary>
    /// Compares a LinkLoXML and this ILoXML for equality.
    /// </summary>
    /// <param name="other">The other LinkLoXML object to compare with.</param>
    /// <returns>True if the two objects are equal, false otherwise.</returns>
    bool EqualsLink(LinkLoXML other);

    /// <summary>
    /// Compares an EmptyLoXML and this ILoXML for equality.
    /// </summary>
    /// <param name="other">The other EmptyLoXML object to compare with.</param>
    /// <returns>True if the two objects are equal, false otherwise.</returns>
    bool EqualsEmpty(EmptyLoXML other);

    /// <summary>
    /// Compares two XML objects for equivalence. Two ILoXML are equivalent if every element of one is equivalent to the corresponding element of the other.
    /// </summary>
    /// <param name="other">The other XML object to compare with.</param>
    /// <returns>True if the two objects are equivalent, false otherwise.</returns>
    bool Equivalent(ILoXML other);

    /// <summary>
    /// Compares a LinkLoXML and this ILoXML for equivalence.
    /// </summary>
    /// <param name="other">The other LinkLoXML object to compare with.</param>
    /// <returns>True if the two objects are equivalent, false otherwise.</returns>
    bool EquivalentLink(LinkLoXML other);

    /// <summary>
    /// Compares an EmptyLoXML and this ILoXML for equivalence.
    /// </summary>
    /// <param name="other">The other EmptyLoXML object to compare with.</param>
    /// <returns>True if the two objects are equivalent, false otherwise.</returns>
    bool EquivalentEmpty(EmptyLoXML other);

    /// <summary>
    /// Gets the plain text representation of this XML object.
    /// </summary>
    /// <returns>The plain text representation of this XML object.</returns>
    string GetPlainText();
}
