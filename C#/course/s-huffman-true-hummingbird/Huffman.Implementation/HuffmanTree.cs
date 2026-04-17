namespace Huffman;

public abstract record HuffmanTree(int Frequency)
{
    /// <summary>
    /// Creates a Huffman tree from a list of words and their frequencies.
    /// </summary>
    /// <param name="words">The words to create the tree from.</param>
    /// <param name="freqs">The frequencies of the words.</param>
    /// <returns>The Huffman tree.</returns>
    public static HuffmanTree From(List<string> words, List<int> freqs) =>
        FromHelper([.. words.Zip(freqs, (word, freq) => new HuffmanLeaf(word, freq))])[0];

    /// <summary>
    /// Creates a list of Huffman trees from a list of leaves.
    /// </summary>
    /// <param name="leaves">The leaves to create the trees from.</param>
    /// <returns>The list of Huffman trees.</returns>
    private static List<HuffmanTree> FromHelper(List<HuffmanTree> leaves)
    {
        if (leaves.Count == 1)
        {
            return leaves;
        }
        var sorted = leaves.OrderBy(leaf => leaf.Frequency);
        var firstTwo = sorted.Take(2).ToArray();
        var (left, right) = (firstTwo[0], firstTwo[1]);

        var node = new HuffmanNode(left.Frequency + right.Frequency, left, right);
        return FromHelper([.. sorted.Skip(2), node]);
    }

    /// <summary>
    /// Encodes a string into a list of bits.
    /// </summary>
    /// <param name="toEncode">The string to encode.</param>
    /// <returns>A list of bits.</returns>
    public List<bool> Encode(string toEncode) =>
        [
            .. toEncode.SelectMany(c =>
            {
                var bools = EncodeCharacter(c.ToString(), []);
                return bools.Count == 0 ? throw new ArgumentOutOfRangeException($"Tried to encode {c} but that is not part of the language.") : bools;
            }),
        ];

    /// <summary>
    /// Encodes a single character into a list of bits.
    /// </summary>
    /// <param name="c">The character to encode.</param>
    /// <param name="path">The path to the current node.</param>
    /// <returns>A list of bits.</returns>
    public abstract List<bool> EncodeCharacter(string c, List<bool> path);

    /// <summary.
    /// Decodes a list of bits into a string.
    /// </summary>
    /// <param name="toDecode">The list of bits to decode.</param>
    /// <returns>A string.</returns>
    public string Decode(List<bool> toDecode)
    {
        if (toDecode.Count == 0)
        {
            return "";
        }

        var (character, rest) = FindCharacter(toDecode);
        return character + Decode(rest);
    }

    /// <summary>
    /// Finds the character from a list of bits and returns the rest of the bits after the character found.
    /// </summary>
    /// <param name="toDecode">The list of bits to decode.</param>
    /// <returns>A tuple containing the character and the rest of the bits.</returns>
    public abstract (string character, List<bool> rest) FindCharacter(List<bool> toDecode);
}

public record HuffmanNode(int Total, HuffmanTree Left, HuffmanTree Right) : HuffmanTree(Total)
{
    public override List<bool> EncodeCharacter(string c, List<bool> path)
    {
        var left = Left.EncodeCharacter(c, [.. path, false]);

        return left.Count == 0 ? Right.EncodeCharacter(c, [.. path, true]) : left;
    }

    public override (string character, List<bool> rest) FindCharacter(List<bool> toDecode)
    {
        if (toDecode.Count == 0)
        {
            return ("?", toDecode);
        }

        List<bool> rest = [.. toDecode.Skip(1)];
        return toDecode[0] ? Right.FindCharacter(rest) : Left.FindCharacter(rest);
    }
}

public record HuffmanLeaf(string Character, int Frequency) : HuffmanTree(Frequency)
{
    public override List<bool> EncodeCharacter(string c, List<bool> path) =>
        c == Character ? path : [];

    public override (string character, List<bool> rest) FindCharacter(List<bool> toDecode) =>
        (Character, toDecode);
}
