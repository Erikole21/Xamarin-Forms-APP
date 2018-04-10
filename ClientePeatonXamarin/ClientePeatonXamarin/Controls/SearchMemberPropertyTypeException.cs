using System;

namespace ClientePeatonXamarin.Controls
{
    public class SearchMemberPropertyTypeException : Exception
    {
        public SearchMemberPropertyTypeException()
        {
        }

        public SearchMemberPropertyTypeException(string message) : base(message)
        {
        }

        public SearchMemberPropertyTypeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
