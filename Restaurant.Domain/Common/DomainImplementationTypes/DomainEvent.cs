﻿using domain.Common.BaseTypes;

namespace domain.Common.DomainImplementationTypes
{
    public abstract record DomainEvent<T>(T Id)
        where T : ValueObject<T>;
}
