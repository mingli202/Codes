using TesterLib;

namespace Huffman;

#pragma warning disable CA1822
class ExamplesHuffman
{
    public bool TestConstructorRejectsMismatchedInputSizes(Tester t)
    {
        return t.CheckConstructorExceptionType(
            typeof(ArgumentOutOfRangeException),
            typeof(HuffmanCodes),
            new List<string>() { "a", "b" },
            new List<int>() { 1 }
        );
    }

    public bool TestConstructorRejectsSingleSymbol(Tester t)
    {
        return t.CheckConstructorExceptionType(
            typeof(ArgumentOutOfRangeException),
            typeof(HuffmanCodes),
            new List<string>() { "a" },
            new List<int>() { 1 }
        );
    }

    public bool TestEncode(Tester t)
    {
        var s = "aaaaaaabbbbiiiiiiiiooooppppp";

        // i: 8
        // a: 7
        // p: 5
        // b: 4
        // o: 4
        //              28
        //            /    \
        //          12      16
        //         / \     / \
        //        p  a    i  8
        //                  / \
        //                 b   o

        var codes = GetCodes(s);
        var encoded = codes.Encode("apobi");

        return t.CheckExpect(encoded, ToListOfBool("010011111010"));
    }

    public bool TestDecode(Tester t)
    {
        var s = "aaaaaaabbbbiiiiiiiiooooppppp";

        // i: 8
        // a: 7
        // p: 5
        // b: 4
        // o: 4
        //              28
        //            /    \
        //          12      16
        //         / \     / \
        //        p  a    i  8
        //                  / \
        //                 b   o

        var codes = GetCodes(s);
        var decoded = codes.Decode(ToListOfBool("010011111010"));

        return t.CheckExpect(decoded, "apobi");
    }

    (List<string> characters, List<int> freqs) GetData(string s)
    {
        var map = s.GroupBy(c => c).ToDictionary(g => g.Key.ToString(), g => g.Count());
        return (map.Keys.ToList(), map.Values.ToList());
    }

    HuffmanCodes GetCodes(string s)
    {
        var (words, freqs) = GetData(s);
        return new HuffmanCodes(words, freqs);
    }

    List<bool> ToListOfBool(string s) => [.. s.Select(i => i != '0')];
}

