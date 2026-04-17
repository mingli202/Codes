using TesterLib;

namespace Webpages;

public record Webpage(string name, ListOfContent content);

public abstract record ListOfContent;
public record EmptyLoContent() : ListOfContent;
public record LinkLoContent(Content first, ListOfContent rest) : ListOfContent;

// A Content is one of
// -- Text
// -- Picture
// -- Hyperlink

public abstract record Content;
public record Text(string name, int numLines, bool inMarkdown) : Content;
public record Picture(string name, string description, double megabytes) : Content;
public record Hyperlink(string text, Webpage destination) : Content;


class ExamplesWebpages
{
    // A website that has at least one text section, two pictures, three webpages, and four hyperlinks, is a list of webpages that contains those constraints.

    Webpage homepage;
    // HashSet<string> visited = new();

    public ExamplesWebpages()
    {
        Webpage Assignment1 = new("Assignment 1",
            new LinkLoContent(new Picture("Submission", "submission screenshot", 13.7),
            new EmptyLoContent())
        );

        Webpage Syllabus = new("Syllabus",
            new LinkLoContent(new Picture("Java", "HD Java logo", 4),
            new LinkLoContent(new Text("Week 1", 10, true),
            new LinkLoContent(new Hyperlink("First Assignment", Assignment1),
            new EmptyLoContent())))
        );

        Webpage Assignments = new("Assignments",
            new LinkLoContent(new Text("Pair Programming", 10, false),
            new LinkLoContent(new Text("Expectations", 15, false),
            new LinkLoContent(new Hyperlink("First Assignment", Assignment1),
            new EmptyLoContent())))
        );


        homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Text("Course Goals", 5, true),
            new LinkLoContent(new Text("Instructor Contact", 1, false),
            new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
            new LinkLoContent(new Picture("Coding Background", "digital rain from the Matrix", 30.2),
            new LinkLoContent(new Hyperlink("Course Syllabus", Syllabus),
            new LinkLoContent(new Hyperlink("Course Assignments", Assignments),
            new EmptyLoContent()))))))
        );
    }

    // Design and implement the methods TotalCredits and PictureInfo here

    /// <summary>
    /// Computes the total number of credits it costs to build this website. Hosting a website costs money, and the web hosting provider gives their rate in terms of credits. Lines of text are free. But pictures are not: For the total number of megabytes (rounded up) of all pictures, each megabyte costs 50 credits.
    /// </summary>
    ///
    /// <example>
    /// homepage = new Webpage("Foundations Homepage",
    ///     new LinkLoContent(new Text("Course Goals", 5, true), new EmptyLoContent())
    /// ); => TotalCredits() => 0
    ///
    /// homepage = new Webpage("Foundations Homepage",
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13), new EmptyLoContent())
    /// ); => TotalCredits() => 1 * 50 = 50
    ///
    /// homepage = new Webpage("Foundations Homepage",
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
    ///     new EmptyLoContent()
    /// ))); => TotalCredits() => 1 * 50 = 50
    ///
    /// homepage = new Webpage("Foundations Homepage",
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
    ///     new EmptyLoContent()
    /// ))); => TotalCredits() => 2 * 50 = 100
    ///
    /// homepage = new Webpage("Foundations Homepage",                            
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),         
    ///     new LinkLoContent(                                                    
    ///         new Hyperlink("Another page", new Webpage("Another page",         
    ///             new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13), 
    ///             new EmptyLoContent()                                          
    ///         ))),                                                              
    ///     new EmptyLoContent()                                                  
    /// ))) => TotalCredits() => 3 * 50 = 150
    ///
    /// homepage = new Webpage("Foundations Homepage",
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),         
    ///     new LinkLoContent(                                                    
    ///         new Hyperlink("Another page", new Webpage("Another page",         
    ///             new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13), 
    ///             new EmptyLoContent()                                          
    ///         ))),                                                              
    ///     new LinkLoContent(                                                    
    ///         new Hyperlink("Another page", new Webpage("Another page",         
    ///             new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13), 
    ///             new EmptyLoContent()
    ///         ))),
    ///     new EmptyLoContent()
    /// )))) => TotalCredits() => 4 * 50 = 200
    /// </example>
    ///
    /// <returns>The total number of credits it costs to build this website.</returns>
    public int TotalCredits(Webpage page) => (int)Math.Ceiling(TotalPictureMegabyes(page.content)) * 50;

    private double TotalPictureMegabyes(ListOfContent content) => content switch
    {
        EmptyLoContent => 0,
        LinkLoContent(var first, var rest) => TotalContentCredits(first) + TotalPictureMegabyes(rest),
        _ => throw new NotImplementedException(),
    };

    private double TotalContentCredits(Content content) => content switch
    {
        Text => 0,
        Picture(_, _, var megabytes) => megabytes,
        Hyperlink(_, var destination) => TotalPictureMegabyes(destination.content),
        _ => throw new NotImplementedException(),
    };


    /// <summary>
    /// Produces one string that has in it the title of all pictures reachable from a webpage, with their description in parentheses, and each separated by comma and space.
    /// </summary>
    ///
    /// <example>
    /// homepage = new Webpage("Foundations Homepage",
    ///     new LinkLoContent(new Text("Course Goals", 5, true), new EmptyLoContent())
    /// ); => PictureInfo() => ""
    ///
    /// homepage = new Webpage("Foundations Homepage",
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13), new EmptyLoContent())
    /// ); => PictureInfo() => "VSCode (VSCode logo)"
    ///
    /// homepage = new Webpage("Foundations Homepage",
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
    ///     new EmptyLoContent()
    /// ))); => PictureInfo() => "VSCode (VSCode logo), VSCode (VSCode logo)"
    ///
    /// homepage = new Webpage("Foundations Homepage",                            
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),         
    ///     new LinkLoContent(                                                    
    ///         new Hyperlink("Another page", new Webpage("Another page",         
    ///             new LinkLoContent(new Picture("VSCode2", "VSCode logo 2", 1.13), 
    ///             new EmptyLoContent()                                          
    ///         ))),                                                              
    ///     new EmptyLoContent()                                                  
    /// ))) => PictureInfo() => "VSCode (VSCode logo), VSCode2 (VSCode logo 2)"
    ///
    /// homepage = new Webpage("Foundations Homepage",
    ///     new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),         
    ///     new LinkLoContent(                                                    
    ///         new Hyperlink("Another page", new Webpage("Another page",         
    ///             new LinkLoContent(new Picture("VSCode2", "VSCode logo2", 1.13), 
    ///             new EmptyLoContent()                                          
    ///         ))),                                                              
    ///     new LinkLoContent(                                                    
    ///         new Hyperlink("Another page", new Webpage("Another page",         
    ///             new LinkLoContent(new Picture("VSCode2", "VSCode logo2", 1.13), 
    ///             new EmptyLoContent()
    ///         ))),
    ///     new EmptyLoContent()
    /// )))) => PictureInfo() => "VSCode (VSCode logo), VSCode2 (VSCode logo 2), VSCode2 (VSCode logo 2)"
    /// </example>
    ///
    /// <returns>A string that has in it the title of all pictures reachable from a webpage, with their description in parentheses, and each separated by comma and space.</returns>
    public string PictureInfo(Webpage page)
    {
        // not arrow function to save result value
        string result = HandlePictureInfo(page.content);
        return result.Length == 0 ? "" : result[..^2];
    }

    /// <summary>
    /// Produces one string that has in it the title of all pictures reachable from a webpage, with their description in parentheses, and each separated by comma and space.
    /// </summary>
    ///
    /// <param name="page">The webpage to produce the string for.</param>
    /// <returns>A string that has in it the title of all pictures reachable from a webpage, with their description in parentheses, and each separated by comma and space.</returns>
    private string HandlePictureInfo(ListOfContent content) => content switch
    {
        EmptyLoContent => "",
        LinkLoContent(var first, var rest) => HandlePictureInfoContent(first) + HandlePictureInfo(rest),
    };

    /// <summary>
    /// Handles content enum
    /// </summary>
    ///
    /// <param name="content">The content to handle.</param>
    /// <returns> The string representation of the content.</returns>
    private string HandlePictureInfoContent(Content content) => content switch
    {
        Text => "",
        Picture(var name, var description, _) => $"{name} ({description}), ",
        Hyperlink(_, var destination) => HandlePictureInfo(destination.content),
        _ => throw new NotImplementedException(),
    };

    // There will be webpages that are double counted because they are referenced by two different pages.
    // And we don't keep track of a visited set, so we can't detect this.

    /* Tests */

    bool TestTotalCredits(Tester t)
    {
        return t.CheckExpect(new ExamplesWebpages().TotalCredits(homepage), 62 * 50);
    }

    bool TestTotalCreditsNoPicture(Tester t)
    {
        ExamplesWebpages webpage = new()
        {
            homepage = new Webpage("Foundations Homepage",
                new LinkLoContent(new Text("Course Goals", 5, true), new EmptyLoContent()))
        };

        return t.CheckExpect(webpage.TotalCredits(webpage.homepage), 0);
    }

    bool TestTotalCreditsOnePicture(Tester t)
    {
        ExamplesWebpages webpage = new()
        {
            homepage = new Webpage("Foundations Homepage",
                new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13), new EmptyLoContent()))
        };

        return t.CheckExpect(webpage.TotalCredits(webpage.homepage), 50);
    }

    bool TestTotalCreditsTwoPictures1(Tester t)
    {
        ExamplesWebpages webpage = new()
        {
            homepage = new Webpage("Foundations Homepage",
                new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
                new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
                new EmptyLoContent()
            )))
        };

        return t.CheckExpect(webpage.TotalCredits(webpage.homepage), 50);
    }

    bool TestTotalCreditsTwoPictures2(Tester t)
    {
        ExamplesWebpages webpage = new()
        {
            homepage = new Webpage("Foundations Homepage",
                new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
                new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
                new EmptyLoContent()
            )))
        };

        return t.CheckExpect(webpage.TotalCredits(webpage.homepage), 100);
    }

    bool TestTotalCreditsWithHyperlink(Tester t)
    {
        ExamplesWebpages webpage = new()
        {
            homepage = new Webpage("Foundations Homepage",
                new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
                new LinkLoContent(
                    new Hyperlink("Another page", new Webpage("Another page",
                        new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
                        new EmptyLoContent()
                    ))),
                new EmptyLoContent()
            )))
        };

        return t.CheckExpect(webpage.TotalCredits(webpage.homepage), 150);
    }

    bool TestTotalCreditsDoubleReferencePage(Tester t)
    {
        ExamplesWebpages webpage = new()
        {
            homepage = new Webpage("Foundations Homepage",
                new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
                new LinkLoContent(
                    new Hyperlink("Another page", new Webpage("Another page",
                        new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
                        new EmptyLoContent()
                    ))),
                new LinkLoContent(
                    new Hyperlink("Another page", new Webpage("Another page",
                        new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
                        new EmptyLoContent()
                    ))),
                new EmptyLoContent()
            ))))
        };

        return t.CheckExpect(webpage.TotalCredits(webpage.homepage), 200);
    }

    bool TestPictureInfo(Tester t)
    {
        return t.CheckExpect(new ExamplesWebpages().PictureInfo(homepage), "VSCode (VSCode logo), Coding Background (digital rain from the Matrix), Java (HD Java logo), Submission (submission screenshot), Submission (submission screenshot)");
    }

    bool TestPictureInfoNoPicture(Tester t)
    {
        ExamplesWebpages webpage = new()
        {
            homepage = new Webpage("Foundations Homepage",
                new LinkLoContent(new Text("Course Goals", 5, true), new EmptyLoContent()))
        };

        return t.CheckExpect(webpage.PictureInfo(webpage.homepage), "");
    }

    bool TestPictureInfoOnePicture(Tester t)
    {
        ExamplesWebpages webpage = new()
        {
            homepage = new Webpage("Foundations Homepage",
                new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13), new EmptyLoContent()))
        };

        return t.CheckExpect(webpage.PictureInfo(webpage.homepage), "VSCode (VSCode logo)");
    }

    bool TestPictureInfoTwoPictures1(Tester t)
    {
        ExamplesWebpages webpage = new()
        {
            homepage = new Webpage("Foundations Homepage",
                new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
                new LinkLoContent(new Picture("VSCode2", "VSCode logo2", 0.13),
                new EmptyLoContent()
            )))
        };

        return t.CheckExpect(webpage.PictureInfo(webpage.homepage), "VSCode (VSCode logo), VSCode2 (VSCode logo2)");
    }

    bool TestPictureInfoWithHyperlink(Tester t)
    {
        ExamplesWebpages webpage = new()
        {
            homepage = new Webpage("Foundations Homepage",
                new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
                new LinkLoContent(
                    new Hyperlink("Another page", new Webpage("Another page",
                        new LinkLoContent(new Picture("VSCode2", "VSCode logo2", 1.13),
                        new EmptyLoContent()
                    ))),
                new EmptyLoContent()
            )))
        };

        return t.CheckExpect(webpage.PictureInfo(webpage.homepage), "VSCode (VSCode logo), VSCode2 (VSCode logo2)");
    }

    bool TestPictureInfoDoubleReferencePage(Tester t)
    {
        ExamplesWebpages webpage = new()
        {
            homepage = new Webpage("Foundations Homepage",
                new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
                new LinkLoContent(
                    new Hyperlink("Another page", new Webpage("Another page",
                        new LinkLoContent(new Picture("VSCode2", "VSCode logo2", 1.13),
                        new EmptyLoContent()
                    ))),
                new LinkLoContent(
                    new Hyperlink("Another page", new Webpage("Another page",
                        new LinkLoContent(new Picture("VSCode2", "VSCode logo2", 1.13),
                        new EmptyLoContent()
                    ))),
                new EmptyLoContent()
            ))))
        };

        return t.CheckExpect(webpage.PictureInfo(webpage.homepage), "VSCode (VSCode logo), VSCode2 (VSCode logo2), VSCode2 (VSCode logo2)");
    }
}

