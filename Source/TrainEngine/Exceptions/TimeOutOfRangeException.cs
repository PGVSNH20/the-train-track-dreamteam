using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Exceptions
{
    public class TimeOutOfRangeException : Exception
    {
        public TimeOutOfRangeException(string message) : base(message)
        {
        }
    }
}