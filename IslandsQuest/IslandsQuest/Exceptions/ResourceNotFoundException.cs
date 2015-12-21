﻿namespace IslandsQuest.Exceptions
{
    using System;

    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message)
            : base(message)
        {
        }
    }
}
