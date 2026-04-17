namespace XML;

public class Attr(string name, string value)
{
    private readonly string _name = name;
    private readonly string _value = value;

    public bool Equals(Attr other) => other._name.Equals(_name) && other._value.Equals(_value);
    public bool SameName(Attr other) => other._name.Equals(_name);
}
