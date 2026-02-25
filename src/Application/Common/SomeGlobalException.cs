using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common;

public class SomeGlobalException : Exception
{
    //  ProblemNotFoundException
}
public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public sealed class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
