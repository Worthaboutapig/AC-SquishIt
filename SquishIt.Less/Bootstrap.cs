using SquishIt.Framework;

namespace SquishIt.Less
{
    using System;
    using dotless.Core;

    public class Bootstrap
    {
        public static void Initialize(Func<ILessEngine> engineBuilder, IPathTranslator pathTranslator)
        {
            Bundle.RegisterStylePreprocessor(new LessPreprocessor(engineBuilder, pathTranslator));
        }
    }
}
