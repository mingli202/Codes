using TesterLib;

namespace XML;

#pragma warning disable CA1822
class ExamplesXML
{
    public bool TestConstructor(Tester t)
    {
        var attributes = new LinkLoAttr(new Attr("class", "container"), new LinkLoAttr(new Attr("class", "width: 100%;"), new EmptyLoAttr()));
        var attributes2 = new LinkLoAttr(new Attr("class", "container"), new LinkLoAttr(new Attr("asdfsadf", "3234"), new LinkLoAttr(new Attr("class", "width: 100%;"), new EmptyLoAttr())));
        return t.CheckConstructorExceptionType(typeof(ArgumentException), typeof(Tag), "tag", attributes, new EmptyLoXML())
            && t.CheckConstructorExceptionType(typeof(ArgumentException), typeof(Tag), "tag", attributes2, new EmptyLoXML());
    }

    public bool TestSameDocument(Tester t)
    {
        var xml1 = new Text("Hello");
        var xml2 = new Text("Hello");
        var xml3 = new Text("Hello world");
        var xml4 = new Text("hello");
        var tag1 = new Tag("tag", new EmptyLoAttr(), new EmptyLoXML());
        var tag2 = new Tag("tag", new EmptyLoAttr(), new EmptyLoXML());

        // var tag3 = CreateTag("tag", new { className = "container" }, []);
        // var tag4 = CreateTag("div", new { className = "container" }, []);
        // var tag5 = CreateTag("div", new { className = "containel" }, []);
        // var tag6 = CreateTag("tag", new { classNama = "container" }, []);
        // var tag7 = CreateTag("Tag", new { className = "container" }, []);

        return t.CheckExpect(xml1.SameDocument(xml2), true)
            && t.CheckExpect(tag1.SameDocument(tag2), true)
            && t.CheckExpect(xml1.SameDocument(tag1), false)
            && t.CheckExpect(xml1.SameDocument(xml3), false)
            && t.CheckExpect(xml1.SameDocument(xml4), false);
        // && t.CheckExpect(tag3.SameDocument(tag4), false)
        // && t.CheckExpect(tag3.SameDocument(tag5), false)
        // && t.CheckExpect(tag3.SameDocument(tag6), false)
        // && t.CheckExpect(tag3.SameDocument(tag7), false);

    }

    public bool TestSameDocumentComplex(Tester t)
    {
        var xml1 = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    new { className= "container", width= "100%", height= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello again")]),
                        CreateTag("p", null, [new Text("Hello once more")]),
                    ]
                )
        ]);


        var same = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    new { className= "container", width= "100%", height= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello again")]),
                        CreateTag("p", null, [new Text("Hello once more")]),
                    ]
                )
        ]);

        var swappedAttributesOrder = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    // height and width swapped
                    new { className= "container", height= "100%", width= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello again")]),
                        CreateTag("p", null, [new Text("Hello once more")]),
                    ]
                )
        ]);

        var differentTagName = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    new { className= "container", width= "100%", height= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello again")]),
                        CreateTag("span", null, [new Text("Hello once more")]),  // span instead of p
                    ]
                )
        ]);


        var differentTextOrder = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    new { className= "container", width= "100%", height= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello once more")]), // swapped with below
                        CreateTag("p", null, [new Text("Hello again")]),
                    ]
                )
        ]);

        var differentContentLength = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    new { className= "container", width= "100%", height= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello again")]),
                        // missing p tag
                    ]
                )
        ]);

        return t.CheckExpect(xml1.SameDocument(same), true)
            && t.CheckExpect(xml1.SameDocument(swappedAttributesOrder), false)
            && t.CheckExpect(xml1.SameDocument(differentTagName), false)
            && t.CheckExpect(xml1.SameDocument(differentTextOrder), false)
            && t.CheckExpect(xml1.SameDocument(differentContentLength), false);
    }

    public bool TestDocumentDifferentAttributeOrder(Tester t)
    {
        var tag1 = CreateTag(
            "article",
            new { className = "container", width = "100%", height = "100%", contentEditable = "true" },
            []
        );

        var tag2 = CreateTag(
            "article",
            new { className = "container", height = "100%", width = "100%", contentEditable = "true" },
            []
        );

        var tag3 = CreateTag(
            "article",
            new { className = "container", height = "100%", width = "100%" },
            []
        );

        return t.CheckExpect(tag1.SameDocument(tag2), false) && t.CheckExpect(tag1.SameDocument(tag3), false);
    }

    public bool TestAttributeValuesMatter(Tester t)
    {
        var tag1 = CreateTag(
            "div",
            new { style = "width: 100%;" },
            []
        );

        var tag2 = CreateTag(
            "div",
            new { style = "width: 50%;" },
            []
        );

        return t.CheckExpect(tag1.SameDocument(tag2), false);
    }

    public bool TestAttributeValuesMatter2(Tester t)
    {
        var tag1 = CreateTag(
            "div",
            new { style = "width: 100%;", className = "container", },
            []
        );

        var tag2 = CreateTag(
            "div",
            new { className = "container", style = "width: 50%;" },
            []
        );

        return t.CheckExpect(tag1.SameXML(tag2), false);
    }

    public bool TestSameText(Tester t)
    {
        var xml1 = new Text("Hello");
        var xml2 = new Text("Hello");
        var xml3 = new Text("Hello world");
        var xml4 = CreateTag("div", new { className = "container", style = "width: 100%;" }, [
            new Text("Hello"),
            new Text(" world"),
        ]);
        var xml5 = CreateTag("div", new { className = "container", style = "width: 100%;" }, [
            new Text("hello"),
            new Text(" world"),
        ]);

        return t.CheckExpect(xml1.SameText(xml2), true)
            && t.CheckExpect(xml1.SameText(xml3), false)
            && t.CheckExpect(xml1.SameText(xml4), false)
            && t.CheckExpect(xml3.SameText(xml4), true)
            && t.CheckExpect(xml4.SameText(xml5), false);
    }

    public bool TestSameTextNested(Tester t)
    {
        var nested = CreateTag("div", null, [
            CreateTag("p", null, [new Text("Hello")]),
            CreateTag("span", null, [new Text(" world")]),
        ]);

        var flat = new Text("Hello world");
        var different = new Text("Hello World");

        return t.CheckExpect(nested.SameText(flat), true)
            && t.CheckExpect(nested.SameText(different), false)
            && t.CheckExpect(flat.SameText(nested), true);
    }

    public bool TestSameXML(Tester t)
    {
        var xml1 = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    new { className= "container", width= "100%", height= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello again")]),
                        CreateTag("p", null, [new Text("Hello once more")]),
                    ]
                )
            ]);


        var same = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    new { className= "container", width= "100%", height= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello again")]),
                        CreateTag("p", null, [new Text("Hello once more")]),
                    ]
                )
        ]);

        var swappedAttributesOrder = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    // height and width swapped
                    new { className= "container", height= "100%", width= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello again")]),
                        CreateTag("p", null, [new Text("Hello once more")]),
                    ]
                )
        ]);

        var differentTagName = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    new { className= "container", width= "100%", height= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello again")]),
                        CreateTag("span", null, [new Text("Hello once more")]),  // span instead of p
                    ]
                )
        ]);


        var differentTextOrder = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    new { className= "container", width= "100%", height= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello once more")]), // swapped with below
                        CreateTag("p", null, [new Text("Hello again")]),
                    ]
                )
        ]);

        var differentContentLength = CreateTag("div", new { className = "container", style = "width= 100%;" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    new { className= "container", width= "100%", height= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello again")]),
                        // missing p tag
                    ]
                )
        ]);


        var differentAttributeLength = CreateTag("div", new { className = "container", style = "width= 100%;", other = "other" }, [
                CreateTag("p", null, [new Text("Hello")]),
                CreateTag("div", new { className= "container", style= "width= 100%;" }, [
                    CreateTag("p", null, [new Text("Hello")]),
                ]),
                CreateTag(
                    "article",
                    new { className= "container", width= "100%", height= "100%", contentEditable= "true" },
                    [
                        CreateTag("p", null, [new Text("Hello")]),
                        CreateTag("p", null, [new Text("Hello World")]),
                        CreateTag("p", null, [new Text("Hello again")]),
                        CreateTag("p", null, [new Text("Hello once more")]),
                    ]
                )
            ]);

        return t.CheckExpect(xml1.SameXML(same), true)
            && t.CheckExpect(xml1.SameXML(swappedAttributesOrder), true)
            && t.CheckExpect(xml1.SameXML(differentTagName), false)
            && t.CheckExpect(xml1.SameXML(differentTextOrder), false)
            && t.CheckExpect(xml1.SameXML(differentContentLength), false)
            && t.CheckExpect(xml1.SameXML(differentAttributeLength), false);
    }

    public bool TestSameXML2(Tester t)
    {

        var tag = CreateTag(
            "article",
            // height and width swapped
            new { className = "container", height = "100%", width = "100%", contentEditable = "true" },
            [
                CreateTag("p", null, [new Text("Hello")]),
            ]
        );

        var tag2 = CreateTag(
            "article",
            new { className = "container", width = "100%", height = "100%", contentEditable = "true" },
            [
                CreateTag("p", null, [new Text("Hello")]),
            ]
        );

        return t.CheckExpect(tag.SameXML(tag2), true);
    }

    private Tag CreateTag(string name, object? attributes, IXML[] content)
    {
        attributes ??= new object();

        Attr[] attrsArray = [.. attributes.GetType().GetProperties().Select(prop => new Attr(prop.Name, prop.GetValue(attributes)?.ToString() ?? ""))];

        var attrs = ILoAttrFromArray(attrsArray);
        var contents = ILoXMLFromArray(content);

        return new Tag(name, attrs, contents);
    }


    private static ILoAttr ILoAttrFromArray(Attr[] attr) => ILoAttrFromArrayInner(attr, 0);
    private static ILoAttr ILoAttrFromArrayInner(Attr[] attr, int index)
        => index >= attr.Length ? new EmptyLoAttr() : new LinkLoAttr(attr[index], ILoAttrFromArrayInner(attr, index + 1));


    private static ILoXML ILoXMLFromArray(IXML[] content) => ILoXMLFromArrayInner(content, 0);
    private static ILoXML ILoXMLFromArrayInner(IXML[] content, int index)
        => index >= content.Length ? new EmptyLoXML() : new LinkLoXML(content[index], ILoXMLFromArrayInner(content, index + 1));


    private bool CheckThrow<T>(Tester t, Func<T> fn)
    {
        try
        {
            fn();
            return t.CheckExpect(false, true, "Expected this method to throw an exception!");
        }
        catch
        {
            return t.CheckExpect(true, true);
        }
    }
}


