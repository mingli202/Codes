namespace UniqueWebpages;

public interface IContent
{
    // returns the name and description of the picture
    (string pictureInfo, string visited) pictureInfo(string visited);

    // number of MBs in pictures the picture
    (double numMegabytes, string visited) numMegabytes(string visited);

    // determines if this content is a Picture that is the same as the given content
    bool isSamePicture(IContent content);
}

public class Text : IContent
{
    readonly private string name;
    readonly private int numLines;
    readonly private bool inMarkdown;

    public Text(string name, int numLines, bool inMarkdown)
    {
        this.name = name;
        this.numLines = numLines;
        this.inMarkdown = inMarkdown;
    }

    public (string pictureInfo, string visited) pictureInfo(string visited) => ("", visited);
    public (double numMegabytes, string visited) numMegabytes(string visited) => (0, visited);
    public bool isSamePicture(IContent content) => false;
}

public class Picture : IContent
{
    readonly private string name;
    readonly private string description;
    readonly private double megabytes;

    public Picture(string name, string description, double megabytes)
    {
        this.name = name;
        this.description = description;
        this.megabytes = megabytes;
    }

    public (string pictureInfo, string visited) pictureInfo(string visited) => ($"{name} ({description})", visited);
    public (double numMegabytes, string visited) numMegabytes(string visited) => (megabytes, visited);
    public bool isSamePicture(IContent content) => false;
};

public class Hyperlink : IContent
{
    readonly private string text;
    readonly private Webpage destination;

    public Hyperlink(string text, Webpage destination)
    {
        this.text = text;
        this.destination = destination;
    }

    public (string pictureInfo, string visited) pictureInfo(string visited) => destination.PictureInfoWithVisited(visited);
    public (double numMegabytes, string visited) numMegabytes(string visited) => destination.NumMegabytes(visited);
    public bool isSamePicture(IContent content) => false;
};


