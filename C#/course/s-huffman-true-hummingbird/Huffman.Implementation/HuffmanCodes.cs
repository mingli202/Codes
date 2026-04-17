namespace Huffman;

public class HuffmanCodes
{
    private HuffmanTree tree;

    public HuffmanCodes(List<string> words, List<int> freqs)
    {
        if (words.Count != freqs.Count)
        {
            throw new ArgumentOutOfRangeException(
                "The number of words and frequencies must match."
            );
        }

        if (words.Count < 2)
        {
            throw new ArgumentOutOfRangeException("There must be at least two letters.");
        }

        tree = HuffmanTree.From(words, freqs);
    }

    /// <summary>
    /// Encodes a string into a list of bits.
    /// </summary>
    /// <param name="toEncode">The string to encode.</param>
    /// <returns>A list of bits.</returns>
    public List<bool> Encode(string toEncode) => tree.Encode(toEncode);

    /// <summary>
    /// Decodes a list of bits into a string.
    /// </summary>
    /// <param name="toDecode">The list of bits to decode.</param>
    /// <returns>A string.</returns>
    public string Decode(List<bool> toDecode) => tree.Decode(toDecode);
}
