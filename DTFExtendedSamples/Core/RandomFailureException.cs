using System;

namespace DTFExtendedSamples.Core
{
    public class RandomFailureException: Exception
    {
        public RandomFailureException(): base("Well, because sometimes things just go wrong...")
        {
        }
    }
}