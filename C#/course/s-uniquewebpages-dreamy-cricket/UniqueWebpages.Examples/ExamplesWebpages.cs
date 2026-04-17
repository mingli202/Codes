using TesterLib;

namespace UniqueWebpages;

class ExamplesWebpages
{
    readonly Webpage homepage;

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
    // Your examples go here!
    //
    bool TestTotalCredits(Tester t)
    {
        return t.CheckExpect(homepage.TotalCredits(), 49 * 50);
    }

    bool TestTotalCreditsNoPicture(Tester t)
    {
        var homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Text("Course Goals", 5, true), new EmptyLoContent()));

        return t.CheckExpect(homepage.TotalCredits(), 0);
    }

    bool TestTotalCreditsOnePicture(Tester t)
    {
        var homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13), new EmptyLoContent()));

        return t.CheckExpect(homepage.TotalCredits(), 50);
    }

    bool TestTotalCreditsTwoPictures1(Tester t)
    {
        var homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
            new LinkLoContent(new Picture("VSCode2", "VSCode2 logo", 0.13),
            new EmptyLoContent()
        )));

        return t.CheckExpect(homepage.TotalCredits(), 50);
    }

    bool TestTotalCreditsTwoPictures2(Tester t)
    {
        var homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
            new LinkLoContent(new Picture("VSCode2", "VSCode2 logo", 0.13),
            new EmptyLoContent()
        )));

        return t.CheckExpect(homepage.TotalCredits(), 100);
    }

    bool TestTotalCreditsWithHyperlink(Tester t)
    {
        var homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
            new LinkLoContent(
                new Hyperlink("Another page", new Webpage("Another page",
                    new LinkLoContent(new Picture("VSCode2", "VSCode2 logo", 1.13),
                    new EmptyLoContent()
                ))),
            new EmptyLoContent()
        )));

        return t.CheckExpect(homepage.TotalCredits(), 150);
    }

    bool TestTotalCreditsDoubleReferencePage(Tester t)
    {
        var page = new Webpage("Another page",
            new LinkLoContent(new Picture("VSCode2", "VSCode2 logo", 1.13),
            new EmptyLoContent()
        ));

        var homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
            new LinkLoContent(
                new Hyperlink("Another page", page),
            new LinkLoContent(
                new Hyperlink("Another page", page),
            new EmptyLoContent()
        ))));

        return t.CheckExpect(homepage.TotalCredits(), 3 * 50);
    }

    bool TestPictureInfo(Tester t)
    {
        var actual = homepage.PictureInfo();
        return t.CheckExpect(actual, "VSCode (VSCode logo), Coding Background (digital rain from the Matrix), Java (HD Java logo), Submission (submission screenshot)");
    }

    bool TestPictureInfoNoPicture(Tester t)
    {
        var homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Text("Course Goals", 5, true), new EmptyLoContent()));

        return t.CheckExpect(homepage.PictureInfo(), "");
    }

    bool TestPictureInfoOnePicture(Tester t)
    {
        var homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13), new EmptyLoContent()));

        return t.CheckExpect(homepage.PictureInfo(), "VSCode (VSCode logo)");
    }

    bool TestPictureInfoTwoPictures1(Tester t)
    {
        var homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Picture("VSCode", "VSCode logo", 0.13),
            new LinkLoContent(new Picture("VSCode2", "VSCode logo2", 0.13),
            new EmptyLoContent()
        )));

        return t.CheckExpect(homepage.PictureInfo(), "VSCode (VSCode logo), VSCode2 (VSCode logo2)");
    }

    bool TestPictureInfoWithHyperlink(Tester t)
    {
        var homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
            new LinkLoContent(
                new Hyperlink("Another page", new Webpage("Another page",
                    new LinkLoContent(new Picture("VSCode2", "VSCode logo2", 1.13),
                    new EmptyLoContent()
                ))),
            new EmptyLoContent()
        )));

        return t.CheckExpect(homepage.PictureInfo(), "VSCode (VSCode logo), VSCode2 (VSCode logo2)");
    }

    bool TestPictureInfoDoubleReferencePage(Tester t)
    {
        var page = new Webpage("Another page",
            new LinkLoContent(new Picture("VSCode2", "VSCode2 logo", 1.13),
            new EmptyLoContent()
        ));

        var homepage = new Webpage("Foundations Homepage",
            new LinkLoContent(new Picture("VSCode", "VSCode logo", 1.13),
            new LinkLoContent(
                new Hyperlink("Another page", page),
            new LinkLoContent(
                new Hyperlink("Another page", page),
            new EmptyLoContent()
        ))));

        return t.CheckExpect(homepage.PictureInfo(), "VSCode (VSCode logo), VSCode2 (VSCode2 logo)");
    }

    bool TestPictureInfoWithManyHyperlinks(Tester t)
    {
        var page1 = new Webpage("page 1",
            new LinkLoContent(new Picture("picture 1", "picture 1", 0), new EmptyLoContent()
        ));

        var page2 = new Webpage("page 3",
            new LinkLoContent(new Picture("picture 2", "picture 2", 0),
            new LinkLoContent(new Text("some text", 10, false),
            new LinkLoContent(new Hyperlink("page 1", page1),
            new EmptyLoContent()
        ))));

        var homepage = new Webpage("homepage",
            new LinkLoContent(new Picture("picture 0", "picture 0", 0),
            new LinkLoContent(new Hyperlink("page 2", page2),
            new EmptyLoContent()
        )));

        var actual = homepage.PictureInfo();

        return t.CheckExpect(actual, "picture 0 (picture 0), picture 2 (picture 2), picture 1 (picture 1)");
    }
}

