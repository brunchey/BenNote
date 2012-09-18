using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenNote.Model.Exceptions
{
    public class ShareListException : Exception
    {
        public ShareListException(string message)
            : base(message) {}
    }
}
