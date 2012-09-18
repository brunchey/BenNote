using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenNote.Model.Exceptions
{
    public class InvalidListVersionException : Exception
    {
        public InvalidListVersionException(string message)
            : base(message)
        {

        }
    }
}
