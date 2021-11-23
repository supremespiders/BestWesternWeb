using System;

namespace ScraperLib.Models
{
    public class KnownException : Exception
    {
        public KnownException(string s) : base(s)
        {
        }
    }
}