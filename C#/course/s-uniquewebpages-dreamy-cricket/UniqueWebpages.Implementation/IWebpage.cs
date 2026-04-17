namespace UniqueWebpages;

public interface IWebpage
{
    /// <summary>
    /// Computes the total number of credits it costs to build this webpage. Hosting a website costs money, and the web hosting provider gives their rate in terms of credits. Lines of text are free. But pictures are not: For the total number of megabytes (rounded up) of all pictures, each megabyte costs 50 credits.
    /// </summary>
    /// <returns>The total number of credits it costs to build this webpage.</returns>
    int TotalCredits();

    /// <summary>
    /// Produces one string that has in it the title of all pictures reachable from this webpage, with their description in parentheses, and each separated by comma and space.
    /// </summary>
    /// <returns>A string that has in it the title of all pictures reachable from this webpage, with their description in parentheses, and each separated by comma and space.</returns>
    string PictureInfo();

    /// <summary>
    /// Produces one string that has in it the title of all pictures reachable from this webpage, with their description in parentheses, and each separated by comma and space. Keeps a string of visited.
    /// </summary>
    /// <returns>A string that has in it the title of all pictures reachable from this webpage, with their description in parentheses, and each separated by comma and space, and a string of visited pages</returns>
    (string pictureInfo, string visited) PictureInfoWithVisited(string visited);

    /// <summary>
    /// Produces one string that has in it the title of all pictures reachable from this webpage, with their description in parentheses, and each separated by comma and space, but starts with a comma. Keeps a string of visited.
    /// </summary>
    /// <returns>A string that has in it the title of all pictures reachable from this webpage, with their description in parentheses, and each separated by comma and space and starts with a comma, and a string of visited pages</returns>
    (string pictureInfo, string visited) PictureInfoWithComma(string visited);

    /// <summary>
    /// Computes the number of megabytes in pictures this webpage. Pictures are not counted if they are visited.
    /// </summary>
    /// <returns>The number of megabytes in pictures this webpage.</returns>
    (double numMegabytes, string visited) NumMegabytes(string visited);

    /// <summary>
    /// Determines if this webpage has been visited given the visited string
    /// </summary>
    /// <returns>True if this webpage has been visited, false otherwise.</returns>
    bool HasBeenVisited(string visited);
}

public class Webpage : IWebpage
{
    readonly private string name;
    readonly private IListOfContent content;

    public Webpage(string name, IListOfContent content)
    {
        this.name = name;
        this.content = content;
    }
    public int TotalCredits() => (int)Math.Ceiling(NumMegabytes("").numMegabytes) * 50;

    public (double numMegabytes, string visited) NumMegabytes(string visited) => HasBeenVisited(visited) ? (0, visited) : content.numMegabytes(visited + name + ",");


    public string PictureInfo() => PictureInfoWithVisited("").pictureInfo;

    public (string pictureInfo, string visited) PictureInfoWithVisited(string visited) => HasBeenVisited(visited) ? ("", visited) : content.pictureInfo(visited + name + ",");

    public (string pictureInfo, string visited) PictureInfoWithComma(string visited) => HasBeenVisited(visited) ? ("", visited) : content.pictureInfoWithComma(visited + name + ",");

    public bool HasBeenVisited(string visited) => visited.Contains(name);
};
