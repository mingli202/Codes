using TesterLib;

namespace XML;

#pragma warning disable CA1822
class ExamplesImplementation
{
    public bool TestSameDocument1(Tester t)
    {
        var xml1 = new Text("Hello");
        var xml2 = new Text("Hello");
        var tag1 = new Tag("tag", new EmptyLoAttr(), new EmptyLoXML());
        var tag2 = new Tag("tag", new EmptyLoAttr(), new EmptyLoXML());
        return t.CheckExpect(xml1.SameDocument(xml2), true)
            && t.CheckExpect(tag1.SameDocument(tag2), true)
            && t.CheckExpect(xml1.SameDocument(tag1), false);
    }

    public bool TestEqualsListAttr(Tester t)
    {
        var list1 = new LinkLoAttr(new Attr("a", "1"), new EmptyLoAttr());
        var list2 = new LinkLoAttr(new Attr("a", "1"), new EmptyLoAttr());
        var list3 = new LinkLoAttr(new Attr("b", "1"), new EmptyLoAttr());
        var list4 = new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("b", "1"), new EmptyLoAttr()));

        return t.CheckExpect(list1.Equals(list2), true)
            && t.CheckExpect(list1.Equals(list3), false)
            && t.CheckExpect(list1.Equals(list4), false);
    }

    public bool TestEqulivalentList(Tester t)
    {
        var list1 = new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("b", "2"), new LinkLoAttr(new Attr("c", "3"), new EmptyLoAttr())));

        var list2 = new LinkLoAttr(new Attr("c", "3"), new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("b", "2"), new EmptyLoAttr())));

        var list3 = new LinkLoAttr(new Attr("c", "3"), new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("d", "4"), new EmptyLoAttr()))));

        return t.CheckExpect(list1.Equivalent(list2), true)
            && t.CheckExpect(list1.Equivalent(list3), false);

    }

    public bool TestEqulivalentList2(Tester t)
    {
        var list1 = new LinkLoAttr(new Attr("className", "container"), new LinkLoAttr(new Attr("height", "100%"), new LinkLoAttr(new Attr("width", "100%"), new LinkLoAttr(new Attr("contentEditable", "true"), new EmptyLoAttr()))));

        var list2 = new LinkLoAttr(new Attr("className", "container"), new LinkLoAttr(new Attr("width", "100%"), new LinkLoAttr(new Attr("height", "100%"), new LinkLoAttr(new Attr("contentEditable", "true"), new EmptyLoAttr()))));


        return t.CheckExpect(list1.Equivalent(list2), true);

    }

    public bool TestAll(Tester t)
    {
        var list1 = new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("a", "1"), new EmptyLoAttr())));
        var list2 = new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("a", "2"), new EmptyLoAttr())));

        return t.CheckExpect(list1.All((attr) => attr.Equals(new Attr("a", "1"))), true)
            && t.CheckExpect(list2.All((attr) => attr.Equals(new Attr("a", "1"))), false);
    }

    public bool TestAny(Tester t)
    {
        var list1 = new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("a", "1"), new EmptyLoAttr())));
        var list2 = new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("a", "2"), new EmptyLoAttr())));

        return t.CheckExpect(list1.Any((attr) => attr.Equals(new Attr("a", "1"))), true)
            && t.CheckExpect(list2.Any((attr) => attr.Equals(new Attr("a", "2"))), true)
            && t.CheckExpect(list2.Any((attr) => attr.Equals(new Attr("a", "3"))), false);
    }

    public bool TestContainsAttr(Tester t)
    {
        var list1 = new LinkLoAttr(new Attr("a", "1"), new LinkLoAttr(new Attr("b", "1"), new LinkLoAttr(new Attr("c", "1"), new EmptyLoAttr())));

        return t.CheckExpect(list1.Contains(new Attr("a", "1")), true)
            && t.CheckExpect(list1.Contains(new Attr("c", "1")), true)
            && t.CheckExpect(list1.Contains(new Attr("d", "3")), false);
    }
}

