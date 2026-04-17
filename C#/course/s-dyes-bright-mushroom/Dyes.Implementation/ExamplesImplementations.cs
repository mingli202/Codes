
using TesterLib;

namespace Dyes;

class ExamplesImplementations
{
    bool TestDarkestRecipe(Tester t)
    {
        return t.CheckConstructorNoException(typeof(DyeRecipe), 1.6D, 4.0D, 0.6D);
    }
}

