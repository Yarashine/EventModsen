using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions;

public class AlreadyParticipatingException : ApplicationException
{
    public AlreadyParticipatingException(string message) : base(message) { }
}
