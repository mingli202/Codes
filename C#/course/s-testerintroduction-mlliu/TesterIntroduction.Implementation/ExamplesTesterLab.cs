namespace TesterIntroduction;

using TesterLib;             // The tester library

class ExamplesTesterLab
{
  // Your tests go here!
  bool TestArithmetic(Tester t)
  {
    return t.CheckExpect(1 + 1, 2);
  }

  bool TestHelloWorldFail(Tester t)
  {
    string a = "This is a test that should fail";
    return t.CheckExpect("Hello world", a);
  }

  string TakesNameAndReturnsHelloPlusName(string name)
  {
    return "Hello, " + name + "!";
  }
}

