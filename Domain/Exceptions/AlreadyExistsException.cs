﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions;

public class AlreadyExistsException : ApplicationException
{
    public AlreadyExistsException(string message) : base(message) { }
}
