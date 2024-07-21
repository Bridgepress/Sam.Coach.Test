using System;

namespace Sam.Coach.Exceptions.Forbidden403
{
    public class LongestIncrementEqualException : ApplicationException
    {
        public LongestIncrementEqualException()
            : base(403, "The longest increment in the two arrays is equal to")
        {
        }
    }
}
