namespace UniqueWebpages;

public interface IListOfContent
{
    // returns the picture info of this list with a comma separated between each picture
    (string pictureInfo, string visited) pictureInfo(string visited);

    // returns the picture info of this list with a comma separated between each picture and a starting comma
    (string pictureInfo, string visited) pictureInfoWithComma(string visited);

    // number of MBs in pictures this list
    (double numMegabytes, string visited) numMegabytes(string visited);
}

public class EmptyLoContent : IListOfContent
{
    public (string pictureInfo, string visited) pictureInfo(string visited) => ("", visited);
    public (string pictureInfo, string visited) pictureInfoWithComma(string visited) => ("", visited);
    public (double numMegabytes, string visited) numMegabytes(string visited) => (0, visited);
}

public class LinkLoContent : IListOfContent
{
    readonly private IContent first;
    readonly private IListOfContent rest;

    public LinkLoContent(IContent first, IListOfContent rest)
    {
        this.first = first;
        this.rest = rest;
    }

    public (string pictureInfo, string visited) pictureInfo(string visited)
    {
        var pictureInfo = first.pictureInfo(visited);

        if (pictureInfo.pictureInfo.Length == 0)
        {
            return rest.pictureInfo(pictureInfo.visited);
        }

        var restPictureInfo = rest.pictureInfoWithComma(pictureInfo.visited);
        return (pictureInfo.pictureInfo + restPictureInfo.pictureInfo, restPictureInfo.visited);
    }


    public (string pictureInfo, string visited) pictureInfoWithComma(string visited)
    {
        var pictureInfo = first.pictureInfo(visited);

        if (pictureInfo.pictureInfo.Length == 0)
        {
            return rest.pictureInfoWithComma(pictureInfo.visited);
        }

        var restPictureInfo = rest.pictureInfoWithComma(pictureInfo.visited);
        return (", " + pictureInfo.pictureInfo + restPictureInfo.pictureInfo, restPictureInfo.visited);
    }

    public (double numMegabytes, string visited) numMegabytes(string visited)
    {
        var first = this.first.numMegabytes(visited);
        var rest = this.rest.numMegabytes(first.visited);

        return (first.numMegabytes + rest.numMegabytes, rest.visited);
    }
}
